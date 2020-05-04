/*
 * Hand_Held_Shock_Tester.c
 *
 * Created: 19-02-2020 13:58:15
 * Author : mikkel
 */ 

#define vref 4.8
#define F_CPU 16000000UL
#define analog_sensitivity 0.3
#define zero_g_voltage 1.65
//include standard libraries
#include <avr/io.h>
#include <stdio.h>
#include <util/delay.h>
#include "i2cmaster.h"
#include "lcd.h"
#include "usart.h"
#include "std.h"

void get_analog_accelerometer(int *x, int *y, int *z);
void convert_raw_to_g(int x_raw, int y_raw, int z_raw, float *x_g, float *y_g, float *z_g);
float conversion(int raw);

int steps_minute[120];


int main(void)
{
	initialse();	
	LCD_init();
	uart_init();
	//io_redirect();
	int x_raw, y_raw, z_raw;
	float x_g, y_g, z_g;
	LCD_set_cursor(0,0);
	printf("HELLO");
	_delay_ms(2000);
	int step_start = 0, current_steps = 0, current_min = 0;
	unsigned long measurement_timer = 0, minutetimer = 0;
	float max = 1, min = 1;
    while (1) 
    {
		if(measurement_timer+10 < millis())
		{
			measurement_timer = millis();
			get_data_accel(&x_raw,&y_raw,&z_raw);
			x_g = (float)x_raw/4096
			if (x_g > max) max = x_g;
			if (x_g < min) min = x_g6;
			LCD_set_cursor(0,4);
			printf("%0.2f   %0.2f",max, min);
			
			if(!step_start)
			{
				if(x_g > 1.5) step_start = 1;
			}
			else
			{
				if(x_g < 0.5)
				{
					step_start = 0;
					current_steps++;
					LCD_clear();
					LCD_set_cursor(0,0);
					printf("%d",current_steps);
				}
			}
			
		}
		
		if(minutetimer+60000 < millis())
		{
			minutetimer = millis();
			steps_minute[current_min] = current_steps;
			current_steps = 0;
		}
	
	/*
	_delay_ms(200);
	//LCD_clear();
	LCD_set_cursor(0,0);
	//	convert_raw_to_g(read_adc(0),read_adc(1), read_adc(2), &x_g, &y_g, &z_g);
	//printf("x=%0.2f y=%0.2f z=%0.2f \n",x_g, y_g, z_g);
	get_data_accel(&x_raw,&y_raw,&z_raw);
	printf("                ");
	LCD_set_cursor(0,0);
	printf("x=%0.2f", (float)x_raw/4096);
	LCD_set_cursor(0,1);
	printf("                ");
	LCD_set_cursor(0,1);
	printf("y=%0.2f", (float)y_raw/4096);
	LCD_set_cursor(0,2);
	printf("                ");
	LCD_set_cursor(0,2);
	printf("z=%0.2f", (float)z_raw/4096);*/
    }
}


void convert_raw_to_g(int x_raw, int y_raw, int z_raw, float *x_g, float *y_g, float *z_g)
{
	*x_g = conversion(x_raw);
	*y_g = conversion(y_raw);
	*z_g = conversion(z_raw);
}

float conversion(int raw)
{
	return (float)((raw * (vref/1024))-zero_g_voltage)/analog_sensitivity;
}

void get_analog_accelerometer(int *x, int *y, int *z)
{
	*x = read_adc(0);
	*y = read_adc(1);
	*z = read_adc(2);
}
