/*
 * bgt60tr13c.h
 *
 *  Created on: 31 May 2024
 *      Author: jorda
 *
 * Rutronik Elektronische Bauelemente GmbH Disclaimer: The evaluation board
 * including the software is for testing purposes only and,
 * because it has limited functions and limited resilience, is not suitable
 * for permanent use under real conditions. If the evaluation board is
 * nevertheless used under real conditions, this is done at one’s responsibility;
 * any liability of Rutronik is insofar excluded
 */

#ifndef BGT60TR13C_H_
#define BGT60TR13C_H_

#include <stdint.h>

int bgt60tr13c_init();

uint16_t bgt60tr13c_is_data_available();

int bgt60tr13c_get_data(uint16_t* data);

uint16_t bgt60tr13c_get_samples_per_frame();

#endif /* BGT60TR13C_H_ */
