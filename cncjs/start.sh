#!/usr/bin/env bash

_stop(){
    kill -9 $child
}

./node_modules/.bin/cncjs > /dev/stdout 2> /dev/stderr

$child =$!

trap _stop TERM
trap _stop KILL

wait $child