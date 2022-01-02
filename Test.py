import subprocess
result = subprocess.run(["/home/pi/mijia/MiTemperature2/LYWSD03MMC.py","-d","A4:C1:38:48:31:DF","-r","-b","-c","1","--name","test01","--httpcallback","http://192.168.1.179:45455/api/v1/XiaomiBLE/mijia/test2?name={sensorname}&temp={temperature}&hum={humidity}&bat={batteryLevel}"]) 
