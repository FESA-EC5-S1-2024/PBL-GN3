#include "include/MQTT.hpp"
#include "include/WI-FI.hpp"
#include "include/config.hpp"
#include "include/measurements.hpp"

#include <Wire.h>
#include <LiquidCrystal_I2C.h>

LiquidCrystal_I2C lcd(0x27, 16, 2);

TaskHandle_t Task1;
TaskHandle_t Task2;

int temperature = 0;
bool publish = false;
unsigned long lastDisplayTime = 0;
const unsigned long displayInterval = 1000; // Intervalo de 1 segundo


void setup() {
  lcd.init();                      // Inicializa o LCD
  lcd.backlight();                 // Liga o backlight do LCD

  InitOutput();
  initSerial();
  initWiFi();
  initMQTT();

  xTaskCreatePinnedToCore(
      Task1code, /* Task function. */
      "Task1",   /* name of task. */
      10000,     /* Stack size of task */
      NULL,      /* parameter of the task */
      1,         /* priority of the task */
      &Task1,    /* Task handle to keep track of created task */
      0);        /* pin task to core 0 */
  delay(500);

  xTaskCreatePinnedToCore(
      Task2code, /* Task function. */
      "Task2",   /* name of task. */
      10000,     /* Stack size of task */
      NULL,      /* parameter of the task */
      1,         /* priority of the task */
      &Task2,    /* Task handle to keep track of created task */
      1);        /* pin task to core 1 */
  delay(500);
}

void Task1code(void *pvParameters) {
  Serial.print("Task1 running on core ");
  Serial.println(xPortGetCoreID());

  while (true) {
    if (publish) {
      String mensagem = String(temperature);
      Serial.print("Valor da temperatura: ");
      Serial.println(mensagem.c_str());
      MQTT.publish(TOPICO_PUBLISH_2, mensagem.c_str());
      publish = false;
    }

    unsigned long currentMillis = millis();
    if (currentMillis - lastDisplayTime >= displayInterval) {
      lcd.setCursor(0, 0);  // Define o cursor do LCD para a primeira linha
      
      lcd.print("Temp: ");
      lcd.print(highResolutionTemperature());  // Exibe a temperatura no LCD
      lcd.print((char)223);
      lcd.print("C");
      
      lastDisplayTime = currentMillis;
    }
  }
}

void Task2code(void *pvParameters) {
  Serial.print("Task2 running on core ");
  Serial.println(xPortGetCoreID());

  while (true) {
    temperature = highResolutionTemperature();
    publish = true;
  }
}

void loop() {}
