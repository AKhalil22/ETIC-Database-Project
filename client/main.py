# Python API client
# main.py = Python script to interact with the API
# test = API test scripts

import requests

# URL of API
BASE_URL = "https://localhost:5000"

# Fetch all data
def get_objects():
    response = requests.get(f"{BASE_URL}/todos")
    
    if response.status_code == 200:
        return response.json() # Return JSON data
    else:
        print(F"@get_object error ({response.status_code}): {response.text}")
        return None


# Create new object (todo)
def create_object(id, name, dueDate, isCompleted):

    data = {"id": id, "name": name, "dueDate": dueDate, "isCompleted": isCompleted}
    response = requests.post(f"{BASE_URL}/todos", json=data)

    if response.status_code == 201:
        print("Object created successfully!")
    else:
        print(F"@create_object error ({response.status_code}): {response.text}")

# API Call examples
objects = get_objects()
if objects:
    print("Objects:", objects)

create_object(1, "Write Demo", "2026-02-22")
