.PHONY: build

build:
	docker build -t atheken/$$(basename $$PWD) .
	docker push atheken/$$(basename $$PWD)