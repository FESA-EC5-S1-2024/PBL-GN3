#include "../include/config.hpp"
#include "../include/measurements.hpp"

void readTemperature(){
  int sensorValue = analogRead(temperaturePin); // Lê o valor analógico do sensor
  float temperature = (sensorValue * (100.0 / 4095.0)) +
                      9.3; // Converte o valor para tensão (0-3.3V)

  // Envia a temperatura para o tópico MQTT correspondente
  char message[10];
  snprintf(message, 10, "%.2f",
           temperature); // Formata a temperatura com 2 casas decimais
  MQTT.publish(TOPICO_PUBLISH_2, message);

  // Exibe a temperatura no monitor serial
  Serial.print("Temperatura: ");
  Serial.print(temperature);

  Serial.println(" graus Celsius");
  Serial.print(sensorValue);
}