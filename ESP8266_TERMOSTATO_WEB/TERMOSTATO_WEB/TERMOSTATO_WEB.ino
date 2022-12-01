

/*            SISTEMA DE CONTROL DE TEMPERATURA MEDIANTE ESP8266           */

#include <Wire.h>
#include <Adafruit_GFX.h>
#include <Adafruit_SSD1306.h>
#include <DHT.h>
#include <DHT_U.h>
#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>
#include <ArduinoJson.h>


//DISPLAY
#define SCREEN_WIDTH 128 // OLED display width, in pixels
#define SCREEN_HEIGHT 64 // OLED display height, in pixels
Adafruit_SSD1306 display(SCREEN_WIDTH, SCREEN_HEIGHT, &Wire, -1);

//PINADO
int pin_Display_SCL = D1;
int pin_Display_SDA = D2;
int pin_DHT_Read = D5;
//int pin_LED_BLUE = D5;
int pin_LED_GREEN = D7;
//int pin_BTN1_IN = D7;
//int pin_BTN2_IN = D8;


//WIFI
const char* ssid     = "";
const char* password = "";
const String NOMBRE = "ESP8266_TEST";
char server[] = "192.168.0.150";
WiFiClient client;

//APP CONFIG
#define DHTTYPE DHT22
DHT_Unified dht(pin_DHT_Read, DHTTYPE);
uint32_t delayMS;
void setup() {
  Serial.begin(115200);
  conectarDisplay();
  configuracion_PINADO();
  delay(3000);
  concectarDHT22();
  delay(1000);
  conectarWIFI();
  delay(1000);
  display.setTextSize(2);
}
void loop() {
  float temperature;
  float humidity;
  delay(delayMS);
  sensors_event_t event;
  
  dht.temperature().getEvent(&event);
  if (isnan(event.temperature)) {
    Serial.println(F("Error reading temperature!"));
  }
  else {
    temperature = event.temperature;
  }
  
  dht.humidity().getEvent(&event);
  if (isnan(event.relative_humidity)) {
    Serial.println(F("Error reading temperature!"));
  }
  else {
    humidity = event.relative_humidity;
  }
  if (temperature != 0 and humidity != 0) {
    NEW_Lectura(temperature, humidity);
    delay(200);
    LED_GREEN(EstadoLed());
  }
  Serial.println(String(temperature, 2) + "  " + String(humidity, 2));
  actualizarDisplay(temperature, humidity, GET_TemperaturaObjetivo());
}
bool EstadoLed() {
  bool respuesta;
  if ((WiFi.status() == WL_CONNECTED)) {
    WiFiClient client;
    HTTPClient http;
    String postData;
    Serial.println("http://192.168.0.150/sh_ws.asmx/GET_Calefaccion_Encendida");
    if (http.begin(client, "http://" + String(server) + "/sh_ws.asmx/GET_Calefaccion_Encendida")) {
      int httpCode = http.GET();
      if (httpCode == HTTP_CODE_OK || httpCode == HTTP_CODE_MOVED_PERMANENTLY) {
        Serial.print(http.getString());
        Serial.print("");
        String payload = http.getString();
        payload.replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>","");
        payload.replace("<boolean xmlns=\"http://tempuri.org/\">","");
        payload.replace("</boolean>","");
        payload.replace("\n","");
        Serial.println("");
        Serial.println(payload.indexOf("true"));
        if(payload.indexOf("true") > 0) {
          Serial.println("True");
          return true;
        }else{
          Serial.println("*" + payload + "*");
          Serial.println("False");
          return false;
        }
      } else {
        Serial.printf("[HTTP] GET... failed, error: %s\n", http.errorToString(httpCode).c_str());
      }
      http.end();
    }
  }
  return respuesta;
}
void NEW_Lectura(float t, float h) {
  if ((WiFi.status() == WL_CONNECTED)) {
    WiFiClient client;
    HTTPClient http;
    String postData;
    postData = "NOMBRE_DISP=" + String(NOMBRE) + "&TEMPERATURA_ACTUAL=" + String(t, 2) + "&HUMEDAD=" + String(h, 2);
    Serial.println("http://192.168.0.150/sh_ws.asmx/NEW_Lectura?" + postData);
    if (http.begin(client, "http://192.168.0.150/sh_ws.asmx/NEW_Lectura?" + postData)) {
      int httpCode = http.GET();
      if (httpCode > 0) {
        Serial.print("Response code: " + httpCode);
        if (httpCode == HTTP_CODE_OK || httpCode == HTTP_CODE_MOVED_PERMANENTLY) {
          String payload = http.getString();
          Serial.println(payload);
        }
      } else {
        Serial.printf("[HTTP] GET... failed, error: %s\n", http.errorToString(httpCode).c_str());
      }
      http.end();
    }
  }
}
double GET_TemperaturaObjetivo() {
  if ((WiFi.status() == WL_CONNECTED)) {
    WiFiClient client;
    HTTPClient http;
    Serial.println("http://192.168.0.150/sh_ws.asmx/GET_TemperaturaObjetivo");
    if (http.begin(client, "http://192.168.0.150/sh_ws.asmx/GET_TemperaturaObjetivo" )) {
      int httpCode = http.GET();
      if (httpCode > 0) {
        if (httpCode == HTTP_CODE_OK || httpCode == HTTP_CODE_MOVED_PERMANENTLY) {
          Serial.print(http.getString());
          Serial.print("");
          String payload = http.getString();
          payload.replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>","");
          payload.replace("<string xmlns=\"http://tempuri.org/\">","");
          payload.replace("</string>","");
          payload.replace("\n","");
          Serial.print(payload);
          Serial.print("");
          return payload.toDouble();
        }
      } else {
        Serial.printf("[HTTP] GET... failed, error: %s\n", http.errorToString(httpCode).c_str());
      }
      http.end();
    }
  }
  return 0;
}
void actualizarDisplay(float t, float h, float Config) {
  display.clearDisplay();
  display.setCursor(0, 10);
  display.println("T:  " + String(t, 2) + "C");
  display.println("H:  " + String(h, 2) + "%");
  display.println("    " + String(Config, 2) + "C");
  display.display();
  delay(1000);
}
void concectarDHT22() {
  dht.begin();
  Serial.println(F("DHT22"));
  // Print temperature sensor details.
  sensor_t sensor;
  dht.temperature().getSensor(&sensor);
  Serial.println(F("------------------------------------"));
  Serial.println(F("Temperature Sensor"));
  Serial.print  (F("Sensor Type: ")); Serial.println(sensor.name);
  Serial.print  (F("Driver Ver:  ")); Serial.println(sensor.version);
  Serial.print  (F("Unique ID:   ")); Serial.println(sensor.sensor_id);
  Serial.print  (F("Max Value:   ")); Serial.print(sensor.max_value); Serial.println(F("°C"));
  Serial.print  (F("Min Value:   ")); Serial.print(sensor.min_value); Serial.println(F("°C"));
  Serial.print  (F("Resolution:  ")); Serial.print(sensor.resolution); Serial.println(F("°C"));
  Serial.println(F("------------------------------------"));
  // Print humidity sensor details.
  dht.humidity().getSensor(&sensor);
  Serial.println(F("Humidity Sensor"));
  Serial.print  (F("Sensor Type: ")); Serial.println(sensor.name);
  Serial.print  (F("Driver Ver:  ")); Serial.println(sensor.version);
  Serial.print  (F("Unique ID:   ")); Serial.println(sensor.sensor_id);
  Serial.print  (F("Max Value:   ")); Serial.print(sensor.max_value); Serial.println(F("%"));
  Serial.print  (F("Min Value:   ")); Serial.print(sensor.min_value); Serial.println(F("%"));
  Serial.print  (F("Resolution:  ")); Serial.print(sensor.resolution); Serial.println(F("%"));
  Serial.println(F("------------------------------------"));
  // Set delay between sensor readings based on sensor details.
  delayMS = sensor.min_delay / 1000;
}
void conectarDisplay() {
  //CONFIGURAR DISPLAY
  if (!display.begin(SSD1306_SWITCHCAPVCC, 0x3C)) { // Address 0x3D for 128x64
    Serial.println(F("SSD1306 allocation failed"));
    for (;;);
  }
  display.clearDisplay();
  display.setTextSize(1);
  display.setTextColor(WHITE);
  display.setCursor(1, 10);
  display.println("CONFIGURACION INICIAL");
  display.display();
}
void conectarWIFI() {
  //WIFI
  display.clearDisplay();
  display.setCursor(1, 10);
  display.println("Connecting to '" + String(ssid) + "'");
  display.println("");
  display.display();

  WiFi.mode(WIFI_STA);
  WiFi.begin(ssid, password);

  while (WiFi.status() != WL_CONNECTED) {
    display.print(".");
    display.display();
    delay(500);
  }
  display.clearDisplay();
  display.setCursor(1, 10);
  display.println(" WiFi connected" );
  delay(1000);
  display.println(WiFi.localIP());
  display.display();
}
void configuracion_PINADO() {
  pinMode(pin_LED_GREEN, OUTPUT);
//  pinMode(pin_BTN1_IN, INPUT);
//  pinMode(pin_BTN2_IN, INPUT);
}
void LED_GREEN(bool activar) {
   Serial.println("Activar => " + String(activar));
  if (activar == 1) {
    digitalWrite(pin_LED_GREEN, HIGH);
    Serial.println("Enciendo Led verde");
  } else {
    digitalWrite(pin_LED_GREEN, LOW);    
    Serial.println("Apago Led verde");
  }
}
