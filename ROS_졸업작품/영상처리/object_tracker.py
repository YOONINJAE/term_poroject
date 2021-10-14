import os
# comment out below line to enable tensorflow logging outputs
os.environ['TF_CPP_MIN_LOG_LEVEL'] = '3'
import time
import tensorflow as tf
physical_devices = tf.config.experimental.list_physical_devices('GPU')
if len(physical_devices) > 0:
    tf.config.experimental.set_memory_growth(physical_devices[0], True)
from absl import app, flags, logging
from absl.flags import FLAGS
import core.utils as utils
from core.yolov4 import filter_boxes
from tensorflow.python.saved_model import tag_constants
from core.config import cfg
from PIL import Image
import cv2
import numpy as np
import matplotlib.pyplot as plt
from tensorflow.compat.v1 import ConfigProto
from tensorflow.compat.v1 import InteractiveSession
# deep sort imports
from deep_sort import preprocessing, nn_matching
from deep_sort.detection import Detection
#from deep_sort.tracker import Tracker
from tools import generate_detections as gdet
flags.DEFINE_string('framework', 'tf', '(tf, tflite, trt')
flags.DEFINE_string('weights', './checkpoints/yolov4-416',
                    'path to weights file')
flags.DEFINE_integer('size', 416, 'resize images to')
flags.DEFINE_boolean('tiny', False, 'yolo or yolo-tiny')
flags.DEFINE_string('model', 'yolov4', 'yolov3 or yolov4')
flags.DEFINE_string('video', './data/video/test.mp4', 'path to input video or set to 0 for webcam')
flags.DEFINE_string('output', None, 'path to output video')
flags.DEFINE_string('output_format', 'XVID', 'codec used in VideoWriter when saving video to file')
flags.DEFINE_float('iou', 0.45, 'iou threshold')
flags.DEFINE_float('score', 0.50, 'score threshold')
flags.DEFINE_boolean('dont_show', False, 'dont show video output')
flags.DEFINE_boolean('info', False, 'show detailed info of tracked objects')
flags.DEFINE_boolean('count', False, 'count objects being tracked on screen')



def calibration(img):
    height, width, channels = img.shape
    mtx = np.array([[634.736, 0., 320.],
                    [0., 634.736, 240.],
                    [0, 0, 1]])
    dist = np.array([-0.378562, 0.192585, 0.001536, 0.004175, 0])
    newmtx, roi = cv2.getOptimalNewCameraMatrix(mtx, dist, (width, height), 1, (width, height))
    newimg = cv2.undistort(img, mtx, dist, None, newmtx)
    x, y, w, h = roi
    newimg = newimg[y:y + h, x:x + w]
    cv2.resize(newimg, (640, 480))

    # 밝기 성분에 대해서만 히스토그램 평활화 수행
    src_ycrcb = cv2.cvtColor(newimg, cv2.COLOR_BGR2YCrCb)
    ycrcb_planes = cv2.split(src_ycrcb)
    ycrcb_planes[0] = cv2.equalizeHist(ycrcb_planes[0])
    dst_ycrcb = cv2.merge(ycrcb_planes)
    dst = cv2.cvtColor(dst_ycrcb, cv2.COLOR_YCrCb2BGR)

    return dst

#"""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""'
#"""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""
#"""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""'
#"""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""
#"""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""'
#"""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""

def yolo(result_points,points_3D,points_2D,encoder,infer,frame,image_data):
    map_2D = cv2.imread("map1.png")
    global pred_bbox
    global boxes
    pred_bbox = None
    boxes = None
    bboxes = None
    batch_data = None
    pred_conf = None
    num_objects = None
    scores, classes, valid_detections = None, None, None

    Homo_mtx, status = cv2.findHomography(points_2D,points_3D)
    
    # run detections on tflite if flag is set
    #print(FLAGS.framework) # = tf
 
    batch_data = tf.constant(image_data)
    pred_bbox = infer(batch_data)
    for key, value in pred_bbox.items():
        boxes = value[:, :, 0:4]
        pred_conf = value[:, :, 4:]
  
    boxes, scores, classes, valid_detections = tf.image.combined_non_max_suppression(boxes=tf.reshape(boxes, (tf.shape(boxes)[0], -1, 1, 4)),scores=tf.reshape(pred_conf, (tf.shape(pred_conf)[0], -1, tf.shape(pred_conf)[-1])),max_output_size_per_class=50,max_total_size=50,iou_threshold=FLAGS.iou,score_threshold=FLAGS.score)

    # convert data to numpy arrays and slice out unused elements
    num_objects = valid_detections.numpy()[0]
    bboxes = boxes.numpy()[0]
    bboxes = bboxes[0:int(num_objects)]
    scores = scores.numpy()[0]
    scores = scores[0:int(num_objects)]
    classes = classes.numpy()[0]
    classes = classes[0:int(num_objects)]

    # format bounding boxes from normalized ymin, xmin, ymax, xmax ---> xmin, ymin, width, height
    original_h, original_w, _ = frame.shape
    bboxes = utils.format_boxes(bboxes, original_h, original_w)
    # store all predictions in one parameter for simplicity when calling functions
    pred_bbox = [bboxes, scores, classes, num_objects]

    # read in all class names from config
    class_names = utils.read_class_names(cfg.YOLO.CLASSES)
    # by default allow all classes in .names file
    allowed_classes = list(class_names.values())
       
    # custom allowed classes (uncomment line below to customize tracker for only people)
    allowed_classes = ['person']
    # loop through objects and use class index to get class name, allow only classes in allowed_classes list
    names = []
    deleted_indx = []
    #print("before",bboxes)
    for i in range(num_objects):
        class_indx = int(classes[i])
        class_name = class_names[class_indx]
        if class_name not in allowed_classes:
            deleted_indx.append(i)
        else:
            names.append(class_name)
    names = np.array(names)
    count = len(names)
   
    # delete detections that are not in allowed_classes
    bboxes = np.delete(bboxes, deleted_indx, axis=0)
    scores = np.delete(scores, deleted_indx, axis=0)

    # encode yolo detections and feed to tracker
    features = encoder(frame, bboxes)
    detections = [Detection(bbox, score, class_name, feature) for bbox, score, class_name, feature in zip(bboxes, scores, names, features)]
    
    #initialize color map
    cmap = plt.get_cmap('tab20b')
    colors = [cmap(i)[:3] for i in np.linspace(0, 1, 20)]

    # run non-maxima supression
    boxs = np.array([d.tlwh for d in detections])
    scores = np.array([d.confidence for d in detections])
    classes = np.array([d.class_name for d in detections])
    indices = preprocessing.non_max_suppression(boxs, classes, 1.0, scores)
    detections = [detections[i] for i in indices]       
    
    # draw bbox on screen
    for x in boxs:

        cv2.rectangle(frame, (int(x[0]), int(x[1])), (int(x[0])+int(x[2]), int(x[1])+int(x[3])), (255,50,0), 2)
        person_x = (int(x[0]) + int(x[0]) + int(x[2])) / 2
        person_y = int(x[1])+ int(x[3]) - 5
        cv2.circle(frame, (int(person_x), int(person_y)), 5, (0,0,220), -1)
        new_x = ((Homo_mtx[0,0]*person_x)+(Homo_mtx[0,1]*person_y)+Homo_mtx[0,2])/((Homo_mtx[2,0]*person_x)+(Homo_mtx[2,1]*person_y)+1)
        new_y = ((Homo_mtx[1,0] * person_x) + (Homo_mtx[1,1] * person_y) + Homo_mtx[1,2]) / ((Homo_mtx[2,0] * person_x) + (Homo_mtx[2,1] * person_y) + 1)

        result_points.append((int(new_x),int(new_y)))
    
    # calculate frames per second of running detections
    
    result = np.asarray(frame)
    result = cv2.cvtColor(frame, cv2.COLOR_RGB2BGR)

   ############################################################################
        #[572, 336], [372, 194], [113, 241], [158, 465]
    #cv2.line(result, (points_2D[0][0],points_2D[0][1]), (points_2D[1][0],points_2D[1][1]), (0, 0, 200), 1)
    #cv2.line(result, (points_2D[1][0],points_2D[1][1]), (points_2D[2][0],points_2D[2][1]), (0, 0, 200), 1)
    #cv2.line(result, (points_2D[2][0],points_2D[2][1]), (points_2D[3][0],points_2D[3][1]), (0, 0, 200), 1)
    #cv2.line(result, (points_2D[3][0],points_2D[3][1]), (points_2D[0][0],points_2D[0][1]), (0, 0, 200), 1)
    
    
    return result, map_2D, result_points
        

#"""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""
#"""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""
#"""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""
#"""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""
#"""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""
#"""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""
def main(_argv):
    # Definition of the parameters
    max_cosine_distance = 0.4
    nn_budget = None
    nms_max_overlap = 1.0
    
    # initialize deep sort
    model_filename = 'model_data/mars-small128.pb'
    encoder = gdet.create_box_encoder(model_filename, batch_size=1)
    # calculate cosine distance metric
    #metric = nn_matching.NearestNeighborDistanceMetric("cosine", max_cosine_distance, nn_budget)
    # initialize tracker
    #tracker = Tracker(metric)

    # load configuration for object detector
    config = ConfigProto()
    config.gpu_options.allow_growth = True
    session = InteractiveSession(config=config)
    STRIDES, ANCHORS, NUM_CLASS, XYSCALE = utils.load_config(FLAGS)
    input_size = FLAGS.size
    video_path = FLAGS.video

   
    # otherwise load standard tensorflow saved model
    
    saved_model_loaded = tf.saved_model.load(FLAGS.weights, tags=[tag_constants.SERVING])
    infer = saved_model_loaded.signatures['serving_default']

    # begin video capture
    try:
        vid = cv2.VideoCapture(0)
        vid2 = cv2.VideoCapture(2)
    except:
        vid = cv2.VideoCapture(video_path)
    
    out = None

    # get video ready to save locally if flag is set
    if FLAGS.output:
        # by default VideoCapture returns float instead of int
        width = int(vid.get(cv2.CAP_PROP_FRAME_WIDTH))
        height = int(vid.get(cv2.CAP_PROP_FRAME_HEIGHT))
        fps = int(vid.get(cv2.CAP_PROP_FPS))
        codec = cv2.VideoWriter_fourcc(*FLAGS.output_format)
        out = cv2.VideoWriter(FLAGS.output, codec, fps, (width, height))

    frame_num = 0
    # while video is running
    while True:

        return_value, frame = vid.read()
        return_value2, frame2 = vid2.read()
        start_time = time.time()
        if return_value:
            frame = calibration(cv2.cvtColor(frame, cv2.COLOR_BGR2RGB))
            image = Image.fromarray(frame)
        else:
            print('Video1 has ended or failed, try a different video format!')
            break
        frame_num +=1
        print('Frame #: ', frame_num)
        
        if return_value:
            frame2 = calibration(cv2.cvtColor(frame2, cv2.COLOR_BGR2RGB))
            image2 = Image.fromarray(frame2)
        else:
            print('Video has ended or failed, try a different video format!')
            break
        frame_num +=1
        print('Frame #: ', frame_num)

        frame_size = frame.shape[:2]
        image_data = cv2.resize(frame, (input_size, input_size))
        image_data = image_data / 255.
        image_data1 = image_data[np.newaxis, ...].astype(np.float32)
        
        frame_size2 = frame2.shape[:2]
        image_data2 = cv2.resize(frame2, (input_size, input_size))
        image_data2 = image_data2 / 255.
        image_data2 = image_data2[np.newaxis, ...].astype(np.float32)
        

   
        #######################################################################################################
        #######################################################################################################map / cam
        result_points = []

        result, map_2D, result_points = yolo(result_points,np.array([[418, 535], [537, 456], [378, 306], [317, 424]]),np.array([[352, 240], [225, 206], [68, 333], [268, 349]]),encoder,infer,frame,image_data1)
        

        result2, map_2D, result_points = yolo(result_points,np.array([[180, 252], [412, 257], [420, 171], [164, 120]]),np.array([[458, 424], [495, 253], [385, 237], [201, 426]]),encoder,infer,frame2,image_data2)

        #print(result_points)
        for p in result_points:
            cv2.circle(map_2D,p,6,(0,220,0),-1)
        cv2.imshow("Video1", result)
        cv2.imshow("Video2", result2)
        cv2.imshow("2D",map_2D)

        #######################################################################################################
        #######################################################################################################
        #######################################################################################################
        fps = 1.0 / (time.time() - start_time)
        print("FPS: %.2f" % fps)
        
        
        
        # if output flag is set, save video file
        
        if cv2.waitKey(1) & 0xFF == ord('q'): break
    cv2.destroyAllWindows()

if __name__ == '__main__':
    try:
        app.run(main)
    except SystemExit:
        pass
