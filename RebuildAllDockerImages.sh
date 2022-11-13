#!bin/bash
sudo docker volume create --name=RabbitMqData
sudo docker volume create --name=MessageMongoDbServerData

sudo docker-compose build --force-rm
