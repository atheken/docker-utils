#!/usr/bin/env bash
echo "Starting socket on '127.0.0.1:9600'..."
exec socat -d -d TCP-LISTEN:9600 "EXEC:'./grbl-sim 1.0 -s /dev/stdout -b /dev/stdout',pty,raw,echo=0"