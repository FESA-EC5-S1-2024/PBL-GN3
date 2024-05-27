#include "../include/config.hpp"
#include "../include/measurements.hpp"

static const int sensorValues[] = {
    541,  561,  639,  718,  794,  905,  983,  1003, 1111, 1200, 1236, 1344,
    1390, 1466, 1572, 1605, 1719, 1760, 1833, 1934, 2016, 2066, 2131, 2231};
static const float temperatures[] = {
    18.5, 20.0, 22.0, 24.0, 26.0, 28.0, 30.0, 32.0, 34.0, 36.0, 38.0, 40.0,
    42.0, 44.0, 46.0, 48.0, 50.0, 52.0, 54.0, 56.0, 58.0, 60.0, 62.0, 64.0};
static const int tableSize = sizeof(sensorValues) / sizeof(sensorValues[0]);

static int readTemperature() { return analogRead(TEMPERATURE_PIN); }

static float interpolate(int sensorValue) {
  if (sensorValue <= sensorValues[0]) {
    return temperatures[0];
  }
  if (sensorValue >= sensorValues[tableSize - 1]) {
    return temperatures[tableSize - 1];
  }

  for (int i = 0; i < tableSize - 1; i++) {
    if (sensorValue < sensorValues[i + 1]) {
      float t = (sensorValue - sensorValues[i]) /
                (float)(sensorValues[i + 1] - sensorValues[i]);
      return temperatures[i] + t * (temperatures[i + 1] - temperatures[i]);
    }
  }
  return temperatures[tableSize - 1];
}

float highResolutionTemperature() {
  int total = 0;

  for (int i = 0; i < SAMPLES; i++) {
    total += readTemperature(); 
    delay(100);
  }

  return interpolate((float)total/SAMPLES);
}

float lowResolutionTemperature() {
  return interpolate(readTemperature());
}


