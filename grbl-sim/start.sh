#!/usr/bin/env bash
echo "Started socket on '/tmp/ttyFAKE'..."
exec socat PTY,raw,link=/tmp/ttyFAKE,echo=0 "EXEC:'./grbl_sim.exe -n -s step.out -b block.out',pty,raw,echo=0"