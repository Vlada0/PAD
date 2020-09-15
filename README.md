# PAD
## Protocol
### Codes 
Each response contains specific status code
- 200 - Operation is successfully done
- 400 - Invalid username
- 401 - User is already subscribed to this topic
- 402 - User is not subscribed to this topic

### Message body
After publisher has connected to broker, it sends message with following format:
```
{"publisherName": PUBLISHER_USERNAME}
```
*If publisher username is free, broker sends response with status code 200 and adds publisher in storage.*\
*Else status code is 400.*
&nbsp;\
&nbsp;\
Broker message format is:
```
{"topic": TOPIC, "message": MESSAGE}
```
When broker handles message from publisher, it detects username by address and adds it to message. Then broker sends this message to subscriber.
```
{"username": USERNAME, "topic": TOPIC, "message": MESSAGE}
```
To receive this message, subscriber must be subscribed to this topic.\
&nbsp;\
To subscribe subscriber sends following message:
```
{"subscribe": TOPIC}
```
*If subscriber is already subscribed to this topic, broker sends response with status code 401.*\
&nbsp;\
To unsubscribe subscriber sends following message:
```
{"unsubscribe": TOPIC}
```
*If subscriber is not subscribed to this topic, broker sends response with status code 402.*

&nbsp;\
Each response has following format:
```
{"statusCode": RESPONSE_STATUS_CODE}
```
