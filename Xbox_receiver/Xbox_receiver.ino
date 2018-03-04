int leds[] = {2,3,4,5};
const int DELAY = 200;

// the setup routine runs once when you press reset:
void setup() {
	// initialize serial communication at 11500 bits per second:
	Serial.begin(11500);

	// make the pushbutton's pin an input:
	
	for( int i = 0; i < 4; i++){
		pinMode(leds[i], OUTPUT);  
		digitalWrite( leds[i], LOW);
	}

	for (int i = 0; i < 2; i++){
		piscarLeds_1();
	}

	acenderLeds();
	delay(DELAY * 5);
	apagarLeds();
}

// the loop routine runs over and over again forever:
void loop() {
	if( Serial.available() > 0){
		int resp = Serial.read();

		if(resp > 47 && resp < 52){
			digitalWrite(leds[resp - 48], HIGH);
		}else if(resp > 51 && resp < 56){
			digitalWrite(leds[resp - 52], LOW);
		}
	}
}

void piscarLeds_1(){ //fazendo circulos
	apagarLeds();
	for(int i = 0; i < 4; i++){
		digitalWrite( leds[i], 1);
		delay( DELAY);
		digitalWrite( leds[i], 0);
		delay( DELAY);
	}
}


void acenderLeds(){
	for (int i = 0; i < 4; i++){
			digitalWrite( leds[i], 1);
	}

}

void apagarLeds(){
	for(int i = 0; i < 4; i++){
		digitalWrite( leds[i], 0);
	}
}

void piscarLeds_2(){ //todos ao mesmo tempo
	for(int i = 0; i < 20; i++){
		acenderLeds();
    
		delay( 3 * DELAY);

		apagarLeds();

		delay( 3 * DELAY);
	}
}

void piscarLed( int i){
	for(int j = 0; j < 20; i++){
		digitalWrite( leds[i], 1);
		delay( DELAY);
		digitalWrite( leds[i], 0);
		delay( DELAY);
	}
}
