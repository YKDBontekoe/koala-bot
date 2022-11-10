#!bin/bash
sudo docker volume create --name=RabbitMqData

sudo docker-compose build --force-rm
