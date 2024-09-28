import sqlite3
import random

# Connect to the DentalClinic database
conn = sqlite3.connect('DentalClinic.db')  # Replace with your database path if needed
cursor = conn.cursor()

# Add a new column 'Description' to the 'Appointments' table
cursor.execute('''
ALTER TABLE Appointments
ADD COLUMN Description TEXT
''')

# List of possible medical descriptions
descriptions = [
    "X-ray image",
    "MRI scan",
    "Examining",
    "Diagnosis",
    "Dental cleaning",
    "Root canal treatment",
    "Teeth extraction",
    "Filling",
    "Orthodontic consultation",
    "Periodontal treatment",
]

# Update each appointment with a random description
cursor.execute('SELECT id FROM Appointments')  # Get all appointment IDs
appointment_ids = cursor.fetchall()

for appointment_id in appointment_ids:
    random_description = random.choice(descriptions)  # Choose a random description
    cursor.execute('''
    UPDATE Appointments
    SET Description = ?
    WHERE id = ?
    ''', (random_description, appointment_id[0]))

# Commit the changes and close the connection
conn.commit()
conn.close()

print("Descriptions added to the Appointments table.")
