import requests # type: ignore
import json

# API URL
BASE_URL = "http://localhost:5245/api/expense"

# GET all expenses
def getExpenses():
    response = requests.get(BASE_URL) # adds /expense endpoint
    if response.status_code == 200:
        print(json.dumps(response.json(), indent = 2))
    else:
        print(f"Error: {response.status_code}, {response.text}")

# Get expense by ID
def getExpenseById(id): # expects id parameter
    response = requests.get(BASE_URL+f"/{id}") # adds /get-expense/{id} endpoint
    if response.status_code == 200:
        print(json.dumps(response.json(), indent = 2))
    else:
        print(f"Error: {response.status_code}, {response.text}")

# POST request to create new expense
def createExpense(new_expense): # expects new expense json parameter
    response = requests.post(BASE_URL, json = new_expense) # adds /create-expense endpoint
    if response.status_code == 200 or 201:
        print(f"Expense created: {response.json()}")
    else:
        print(f"Error: {response.status_code}, {response.text}")

# Put (Update): api/expense/{id} request to update expense
def updateExpense(id, updated_expense): # expects id and updated expense json parameters
    response = requests.put(BASE_URL+f"/{id}", json = updated_expense) # adds /update-expense/{id} endpoint

    if response.status_code == 200 or 204 or 201:
        print(f"Expense updated: {response.json()}")
    elif response.status_code == 404:
        print("Error: Expense not found.")
    else:
        print(f"Error: {response.status_code}, {response.text}")

# DELETE request to delete expense
def deleteExpense(id): # expects id parameter
    response = requests.delete(BASE_URL+f"/{id}") # adds /delete-expense/{id} endpoint
    if response.status_code == 204:
        print(f"Expense deleted: {response.json()}")
    else:
        print(f"Error: {response.status_code}, {response.text}")

# Allow users to perform CRUD operations with inputs
def run():
    while True:
        user_input = input("1. Get All Expenses\n2. Get Expense by Id\n3. Create New Expense\n4. Update Expense\n5. Delete Expense\n6. Exit\n" ) # Command list for CRUD operations
        user_input = int(user_input) # Cast user input to int

        if user_input == 1:
            getExpenses()

        elif user_input == 2:
            id = input("Enter ID: ")
            getExpenseById(id)

        elif user_input == 3:

            description = input("Enter Description: ")
            amount = input("Enter Cost: ")

            new_expense = {
                "Description": description,
                "Amount": amount
            }

            createExpense(new_expense)

        elif user_input == 4:

            id = input("Enter ID: ")
            updated_description = input("Enter Updated Description: ")
            updated_amount = input("Enter Updated Cost: ")

            updated_expense = {
                "Id" : id,
                "Description": updated_description,
                "Amount": updated_amount
            }

            updateExpense(id, updated_expense)

        elif user_input == 5:

            id = input("Enter ID for Deletion: ")
            deleteExpense(id)

        elif user_input == 6:
            break # End loop

if __name__ == "__main__":
    run()