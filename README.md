# HttpLauncher

HttpLauncher is a Windows desktop application that runs in the background and launches arbitrary programs
when it is called over its embedded HTTP server.

It is meant to provide easy integration between your Windows computers and 'web hooks' used by services
like [IFTT](https://iftt.com) or [Jeedom](https://jeedom.fr), e.g. for home automation scenarios.

## Usage

1. Start HttpLauncher.
2. Configure the HTTP port and API key if necessary.
3. Click the `Start` button to start the HTTP server.
4. Define applications to run using the `Add...` button.
5. Launch applications by calling URLs of the form: `http://<API key>@<host name>:<port>/launch?app=<name>`

## Download binary

A precompiled executable of the current master is available for download
[here](http://files.crespel.me/oss/HttpLauncher.exe).

## Building from source

This project can be built from source using [Visual Studio](https://www.visualstudio.com).
