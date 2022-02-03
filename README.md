# cutnet
a small console application that quickly disables all connected network adapters or displays info about all local network adapters

## Usage

```
cutnet enable/disable
```
This enables/disables all Ethernet based network adapters based on their names. If the word "WLAN" or "Ethernet" is part of their name, the adapter is enabled/disabled.

```
cutnet info
```
Displays information about all present network adapters of the local system.
