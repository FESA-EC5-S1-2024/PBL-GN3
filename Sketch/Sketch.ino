#include "include/MQTT.hpp"
#include "include/WiFi.hpp"
#include "include/config.hpp"
#include "include/measurements.hpp"

TaskHandle_t Task1;
TaskHandle_t Task2;

float temperature = 0;
bool publishMQTT = false;

void setup() {
  InitOutput(); // Inicializa as saídas do sistema
  initSerial(); // Inicializa a comunicação serial
  initWiFi();   // Inicializa a conexão WiFi
  initMQTT();   // Inicializa a comunicação MQTT

  // Cria e inicia a Task1 no core 0
  xTaskCreatePinnedToCore(Task1code, "Task1", 10000, NULL, 1, &Task1, 0);
  delay(500);

  // Cria e inicia a Task2 no core 1
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
      VerificaConexoesWiFIEMQTT(); // Verifica conexões WiFi e MQTT
      MQTT.loop();                 // Mantém a comunicação MQTT
    }
    delay(100); // Necessário para o watchdog não desligar o ESP32
  }
}

void Task2code(void *pvParameters) {
  Serial.print("Task2 running on core ");
  Serial.println(xPortGetCoreID());

  while (true) {
    temperature =
        highResolutionTemperature(); // Obtém a temperatura com alta resolução
    publishMQTT = true;              // Habilita a publicação MQTT
  }
}

void loop() {
  // Função loop vazia, todo o código é executado pelas tasks
  // Necessário para compilar
}
