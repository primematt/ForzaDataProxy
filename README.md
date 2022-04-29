# ForzaDataProxy
A small proxy for Forza Horizon 5 Data Out data.

## Configuration
Configure the service in `appsettings.json`.

**Example Configuration**
```
  "ForzaDataProxy": {
    "BindAddress": "0.0.0.0",
    "ListenPort": 4950,
    "DataOutPort": 5300,
    "Capture": {
      "Enabled": true,
      "DrivingOnly": true,
      "SavePath": "E:\\Recording"
    }
  }
```

**BindAddress**  
The IP Address to bind the Listen server to.

**ListenPort**  
The port to use to listen for connections.

**DataOutPort**  
The port to connect to Forza with - this should be the same as set in the Forza Options.

**Capture:Enabled**  
Enable capturing of data packets, and writing them to disk.

**Capture:DrivingOnly**  
Only capture packets when 'driving' - ie. not in menus.

**Capture:SavePath**  
The folder to save capture files to.
