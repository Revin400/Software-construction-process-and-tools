# See https://linuxize.com/post/curl-rest-api

curl -X GET http://127.0.0.1:8080/api/v1/warehouses

curl -X POST -H "Content-Type: application/json" -d '{
        "id": 3,
        "code": "TEST",
        "name": "Warehousing Test",
        "address": "Wegstraat 111",
        "zip": "1111 TT",
        "city": "Rotterdam",
        "province": "Zuid-Holland",
        "country": "NL",
        "contact": {
            "name": "John Test",
            "phone": "06-12345678",
            "email": "john@warehousing.test"
        },
        "created_at": "2024-02-03 13:12:00",
        "updated_at": "2024-02-03 13:12:00"}' http://127.0.0.1:8080/api/v1/warehouses