.PHONY: build

TAG ?= latest

build:
	docker build -t atheken/$$(basename $$PWD):$(TAG) .
	docker push atheken/$$(basename $$PWD):$(TAG)