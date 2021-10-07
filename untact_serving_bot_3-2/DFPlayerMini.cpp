
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

#include "DFPlayerMini.h"

DFPlayerMini::DFPlayerMini(PinName tx, PinName rx): mp3(tx, rx)
{
    uint8_t tmp[10] = {0x7E, 0xFF, 06, 00, 00, 00, 00, 00, 00, 0xEF};
    memcpy(send_buf, tmp, 10);
    is_reply = 0;
    mp3.format(8,Serial::None,1);
    mp3.baud(9600);
}

void DFPlayerMini::mp3_set_reply (uint8_t state)
{
    is_reply = state;
    send_buf[4] = is_reply;
}

void DFPlayerMini::fill_uint16_bigend (uint8_t *thebuf, uint16_t data)
{
    *thebuf =   (uint8_t)(data>>8);
    *(thebuf+1) =   (uint8_t)data;
}

//calc checksum (1~6 byte)
uint16_t DFPlayerMini::mp3_get_checksum (uint8_t *thebuf)
{
    uint16_t sum = 0;
    for (int i=1; i<7; i++) {
        sum += thebuf[i];
    }
    return -sum;
}


//fill checksum to send_buf (7~8 byte)
void DFPlayerMini::mp3_fill_checksum ()
{
    uint16_t checksum = mp3_get_checksum (send_buf);
    fill_uint16_bigend (send_buf+7, checksum);
}

void DFPlayerMini::send_func ()
{
    int i;
    for (i = 0; i < 10; i++) {
        mp3.putc(send_buf[i]);
    }
}

void DFPlayerMini::mp3_send_cmd (uint8_t cmd, uint16_t arg)
{
    send_buf[3] = cmd;
    fill_uint16_bigend ((send_buf+5), arg);
    mp3_fill_checksum ();
    send_func ();
}

void DFPlayerMini::mp3_send_cmd (uint8_t cmd)
{
    send_buf[3] = cmd;
    fill_uint16_bigend ((send_buf+5), 0);
    mp3_fill_checksum ();
    send_func ();
}


void DFPlayerMini::mp3_play_physical (uint16_t num)
{
    mp3_send_cmd (0x03, num);
}

void DFPlayerMini::mp3_play_physical ()
{
    mp3_send_cmd (0x03);
}

void DFPlayerMini::mp3_next ()
{
    mp3_send_cmd (0x01);
}

void DFPlayerMini::mp3_prev ()
{
    mp3_send_cmd (0x02);
}
// increase volume
void DFPlayerMini::mp3_volumeplus(){
     mp3_send_cmd(0x04); 
}

// decrease volume
void DFPlayerMini::mp3_volumeminus(){
     mp3_send_cmd(0x05);    
}

//0x06 set volume 0-30
void DFPlayerMini::mp3_set_volume (uint16_t volume)
{
    mp3_send_cmd (0x06, volume);
}

//0x07 set EQ0/1/2/3/4/5    Normal/Pop/Rock/Jazz/Classic/Bass
void DFPlayerMini::mp3_set_EQ (uint16_t eq)
{
    mp3_send_cmd (0x07, eq);
}

//0x09 set device 1/2/3/4/5 U/SD/AUX/SLEEP/FLASH
void DFPlayerMini::mp3_set_device (uint16_t device)
{
    mp3_send_cmd (0x09, device);
}

void DFPlayerMini::mp3_sleep ()
{
    mp3_send_cmd (0x0a);
}

void DFPlayerMini::mp3_reset ()
{
    mp3_send_cmd (0x0c);
}

void DFPlayerMini::mp3_play ()
{
    mp3_send_cmd (0x0d);
}

void DFPlayerMini::mp3_pause ()
{
    mp3_send_cmd (0x0e);
}

void DFPlayerMini::mp3_stop ()
{
    mp3_send_cmd (0x16);
}

// play mp3 file in mp3 folder in your tf card
void DFPlayerMini::mp3_play (uint16_t num)
{
    mp3_send_cmd (0x12, num);
}

void DFPlayerMini::mp3_get_state ()
{
    mp3_send_cmd (0x42);
}

void DFPlayerMini::mp3_get_volume ()
{
    mp3_send_cmd (0x43);
}

void DFPlayerMini::mp3_get_u_sum ()
{
    mp3_send_cmd (0x47);
}

void DFPlayerMini::mp3_get_tf_sum ()
{
    mp3_send_cmd (0x48);
}

void DFPlayerMini::mp3_get_flash_sum ()
{
    mp3_send_cmd (0x49);
}

void DFPlayerMini::mp3_get_tf_current ()
{
    mp3_send_cmd (0x4c);
}

void DFPlayerMini::mp3_get_u_current ()
{
    mp3_send_cmd (0x4b);
}


//
void DFPlayerMini::mp3_get_flash_current ()
{
    mp3_send_cmd (0x4d);
}

void DFPlayerMini::mp3_single_loop (uint8_t state)
{
    mp3_send_cmd (0x19, !state);
}

void DFPlayerMini::mp3_single_play (uint16_t num)
{
    mp3_play (num);
    wait_ms (10);
    mp3_single_loop (true);
}

void DFPlayerMini::mp3_DAC (uint8_t state)
{
    mp3_send_cmd (0x1a, !state);
}

//
void DFPlayerMini::mp3_random_play ()
{
    mp3_send_cmd (0x18);
}
