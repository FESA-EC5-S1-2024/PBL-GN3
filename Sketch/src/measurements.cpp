#include "../include/config.hpp"       // Inclui o arquivo de configuração
#include "../include/measurements.hpp" // Inclui o arquivo de medições

// Valores do sensor e temperaturas correspondentes
static const int sensorValues[] = {
    541,  544,  561,  601,  628,  707,  743,  777,  798,  860,  888,
    943,  976,  1008, 1044, 1076, 1123, 1152, 1194, 1198, 1235, 1270,
    1312, 1384, 1422, 1462, 1493, 1507, 1555, 1600, 1646, 1677, 1718,
    1747, 1793, 1834, 1871, 1905, 1947, 1982, 2005, 2055, 2089, 2119,
    2164, 2199, 2231, 2283, 2337, 2353, 2389, 2433};
static const float temperatures[] = {
    18.5, 19.0, 20.0, 21.0, 22.0, 23.0, 24.0, 25.0, 26.0, 27.0, 28.0,
    29.0, 30.0, 31.0, 32.0, 33.0, 34.0, 35.0, 36.0, 37.0, 38.0, 39.0,
    40.0, 41.0, 42.0, 43.0, 44.0, 45.0, 46.0, 47.0, 48.0, 49.0, 50.0,
    51.0, 52.0, 53.0, 54.0, 55.0, 56.0, 57.0, 58.0, 59.0, 60.0, 61.0,
    62.0, 63.0, 64.0, 65.0, 66.0, 67.0, 68.0, 69.0};
static const int tableSize = sizeof(sensorValues) / sizeof(sensorValues[0]);

// Coeficientes do filtro FIR
#define FIR_ORDER 5
static const float firCoefficients[FIR_ORDER] = {0.2, 0.2, 0.2, 0.2, 0.2};

// Buffer do sensor e índice do buffer
static int sensorBuffer[FIR_ORDER] = {0};
static int bufferIndex = 0;

// Função para ler o valor do sensor de temperatura
static int readTemperature() { return analogRead(TEMPERATURE_PIN); }

// Função para interpolar o valor da temperatura
static float interpolate(int sensorValue) {
  // Se o valor do sensor for menor ou igual ao primeiro valor da tabela,
  // retorna a primeira temperatura da tabela
  if (sensorValue <= sensorValues[0]) {
    return temperatures[0];
  }
  // Se o valor do sensor for maior ou igual ao último valor da tabela,
  // retorna a última temperatura da tabela
  if (sensorValue >= sensorValues[tableSize - 1]) {
    return temperatures[tableSize - 1];
  }

  // Interpola linearmente entre os valores da tabela para obter a temperatura
  for (int i = 0; i < tableSize - 1; i++) {
    if (sensorValue < sensorValues[i + 1]) {
      float t = (sensorValue - sensorValues[i]) /
                (float)(sensorValues[i + 1] - sensorValues[i]);
      return temperatures[i] + t * (temperatures[i + 1] - temperatures[i]);
    }
  }
  // Se não encontrar um intervalo adequado na tabela, retorna a última
  // temperatura
  return temperatures[tableSize - 1];
}

// Função para aplicar o filtro FIR
static float applyFIRFilter(int newValue) {
  // Adiciona o novo valor ao buffer e atualiza o índice do buffer
  sensorBuffer[bufferIndex] = newValue;
  bufferIndex = (bufferIndex + 1) % FIR_ORDER;

  // Calcula o valor filtrado multiplicando os valores do buffer pelos
  // coeficientes do filtro
  float filteredValue = 0;
  for (int i = 0; i < FIR_ORDER; i++) {
    int index = (bufferIndex + i) % FIR_ORDER;
    filteredValue += sensorBuffer[index] * firCoefficients[i];
  }

  return filteredValue;
}

// Função para obter a temperatura com alta resolução
float highResolutionTemperature() {
  unsigned long total = 0;
  unsigned long startTime = millis();
  int sampleCount = 0;

  // Realiza a média de várias leituras do sensor com aplicação do filtro
  while (sampleCount < SAMPLES) {
    // Realiza uma leitura do sensor a cada 50ms
    if (millis() - startTime >= 50) {
      int currentValue = readTemperature();
      float filteredValue = applyFIRFilter(currentValue);
      total += filteredValue;
      sampleCount++;
      startTime = millis();
    }
  }

  // Calcula a média dos valores filtrados e interpola para obter a temperatura
  // final
  return interpolate((float)total / SAMPLES);
}
