#include "include/MQTT.hpp"
#include "include/WiFi.hpp"
#include "include/config.hpp"
#include "include/measurements.hpp"

TaskHandle_t Task1;
TaskHandle_t Task2;

float temperature = 0;
bool publishMQTT = false;

void setup() {
  InitOutput();
  initSerial();
  initWiFi();
  initMQTT();

  xTaskCreatePinnedToCore(Task1code, "Task1", 10000, NULL, 1, &Task1, 0);
  delay(500);

  xTaskCreatePinnedToCore(Task2code, "Task2", 10000, NULL, 1, &Task2, 1);
  delay(500);
}

void Task1code(void *pvParameters) {
  Serial.print("Task1 running on core ");
  Serial.println(xPortGetCoreID());

  while (true) {
    if (publishMQTT) {
      String mensagem = String(temperature);
      Serial.print("Valor da temperatura: ");
      Serial.println(mensagem.c_str());
      MQTT.publish(TOPICO_PUBLISH, mensagem.c_str());
      publishMQTT = false;
      VerificaConexoesWiFIEMQTT();
      MQTT.loop();
    }
  }
}

void Task2code(void *pvParameters) {
  Serial.print("Task2 running on core ");
  Serial.println(xPortGetCoreID());

  while (true) {
    temperature = highResolutionTemperature();
    publishMQTT = true;
  }
}

void loop() {}
