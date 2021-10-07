/*******************************************************************************
 * Copyright (C) 2014 DFRobot                              *
 *                                         *
 * DFPlayer_Mini_Mp3, This library provides a quite complete function for      * 
 * DFPlayer mini mp3 module.                                                   *
 * www.github.com/dfrobot/DFPlayer_Mini_Mp3 (github as default source provider)*
 *  DFRobot-A great source for opensource hardware and robot.                  *
 *                                                                             *
 * This file is part of the DFplayer_Mini_Mp3 library.                         *
 *                                                                             *
 * DFPlayer_Mini_Mp3 is free software: you can redistribute it and/or          *
 * modify it under the terms of the GNU Lesser General Public License as       *
 * published by the Free Software Foundation, either version 3 of              *
 * the License, or any later version.                                          *
 *                                                                             *
 * DFPlayer_Mini_Mp3 is distributed in the hope that it will be useful,        *
 * but WITHOUT ANY WARRANTY; without even the implied warranty of              *
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the               *
 * GNU Lesser General Public License for more details.                         *
 *                                                                             *
 * DFPlayer_Mini_Mp3 is distributed in the hope that it will be useful,        *
 * but WITHOUT ANY WARRANTY; without even the implied warranty of              *
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the               *
 * GNU Lesser General Public License for more details.                         *
 *                                                                             *
 * You should have received a copy of the GNU Lesser General Public            *
 * License along with DFPlayer_Mini_Mp3. If not, see                           *
 * <http://www.gnu.org/licenses/>.                                             *
 ******************************************************************************/
 
// ===========================================================================
// DFPlayerMini.cpp
// Nov 23 2016, kysiki
// ===========================================================================
// Just a simple library for DFPlayer Mini porting from DFPlayer library V2.0.
// (https://www.dfrobot.com/wiki/index.php/DFPlayer_Mini_SKU:DFR0299)
#ifndef MBED_SHIFTREG_H
#define MBED_DFPLAYERMINI_H
#include "mbed.h"
class DFPlayerMini {
public:
    DFPlayerMini(PinName tx, PinName rx );
    void mp3_set_reply (uint8_t state);
    void mp3_play_physical (uint16_t num);
    void mp3_play_physical ();
    void mp3_next ();
    void mp3_prev ();
    void mp3_set_volume (uint16_t volume);
    void mp3_set_EQ (uint16_t eq);
    void mp3_set_device (uint16_t device);
    void mp3_sleep ();
    void mp3_reset ();
    void mp3_play ();
    void mp3_pause ();
    void mp3_stop ();
    void mp3_play (uint16_t num);
    void mp3_get_state ();
    void mp3_get_volume ();
    void mp3_get_u_sum ();
    void mp3_get_tf_sum ();
    void mp3_get_flash_sum ();
    void mp3_get_tf_current ();
    void mp3_get_u_current ();
    void mp3_get_flash_current ();
    void mp3_single_loop (uint8_t state);
    void mp3_single_play (uint16_t num);
    void mp3_DAC (uint8_t state);
    void mp3_random_play ();
    void mp3_volumeplus();
    void mp3_volumeminus();

    Serial mp3;  
    uint8_t send_buf[10];
    uint8_t recv_buf[10];
    bool is_reply;
    static void fill_uint16_bigend (uint8_t *thebuf, uint16_t data);
    uint16_t mp3_get_checksum (uint8_t *thebuf);
    void mp3_fill_checksum ();
    void send_func ();
    void mp3_send_cmd (uint8_t cmd, uint16_t arg);
    void mp3_send_cmd (uint8_t cmd);
};
 
#endif
