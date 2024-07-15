/*
 * bgt60tr13c.h
 *
 *  Created on: 31 May 2024
 *      Author: jorda
 */

#ifndef BGT60TR13C_H_
#define BGT60TR13C_H_

#include <stdint.h>

int bgt60tr13c_init();

uint16_t bgt60tr13c_is_data_available();

int bgt60tr13c_get_data(uint16_t* data);

uint16_t bgt60tr13c_get_samples_per_frame();

#endif /* BGT60TR13C_H_ */
