import cv2
from deepface import DeepFace
from flask import Flask, jsonify, Response
import threading
import base64

# Initialize Flask app
app = Flask(__name__)

# Load face cascade classifier
face_cascade = cv2.CascadeClassifier(cv2.data.haarcascades + 'haarcascade_frontalface_default.xml')

# Create global variables to store emotion and video capture object
detected_emotion = {}
cap = None

@app.route('/emotion', methods=['GET'])
def get_emotion():
    return jsonify(detected_emotion)

@app.route('/frame', methods=['GET'])
def get_frame():
    global cap
    # Capture frame-by-frame
    ret, frame = cap.read()

    if not ret:
        return jsonify({'error': 'Failed to capture frame'})

    # Convert frame to JPG format and then to Base64
    _, buffer = cv2.imencode('.jpg', frame)
    frame_base64 = base64.b64encode(buffer).decode('utf-8')

    # Send base64 encoded frame
    return jsonify({'frame': frame_base64})

def analyze_emotion():
    global detected_emotion
    global cap
    cap = cv2.VideoCapture(0)

    while True:
        ret, frame = cap.read()

        if not ret:
            break

        # Convert frame to grayscale
        gray_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

        # Convert grayscale frame to RGB format
        rgb_frame = cv2.cvtColor(gray_frame, cv2.COLOR_GRAY2RGB)

        # Detect faces in the frame
        faces = face_cascade.detectMultiScale(gray_frame, scaleFactor=1.1, minNeighbors=5, minSize=(30, 30))

        for (x, y, w, h) in faces:
            # Extract the face ROI (Region of Interest)
            face_roi = rgb_frame[y:y + h, x:x + w]

            # Perform emotion analysis on the face ROI
            result = DeepFace.analyze(face_roi, actions=['emotion'], enforce_detection=False)

            # Since DeepFace.analyze() returns a list, access the first element
            if isinstance(result, list):
                result = result[0]

            emotion = result['emotion']  # Now access the 'emotion' key

            # Round emotion percentages
            for e in emotion:
                emotion[e] = round(emotion[e])

            # Update the detected emotion dictionary
            detected_emotion = emotion

        # Show the frame with a rectangle (for visual feedback if needed)
        cv2.imshow('Real-time Emotion Detection', frame)secretmessage_2c14.txt

        # Press 'q' to exit
        if cv2.waitKey(1) & 0xFF == ord('q'):
            break

    cap.release()
    cv2.destroyAllWindows()

if __name__ == '__main__':
    from threading import Thread
    # Run the Flask app in a separate thread
    thread = Thread(target=app.run, kwargs={'port': 5000})
    thread.start()

    # Start analyzing emotions
    analyze_emotion()
