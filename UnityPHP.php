<?php
// Database configuration
$servername = "localhost";
$username = "laiapp4";
$password = "wEa6PS8XgQ7b";
$database = "laiapp4";

// Create a database connection
$conn = new mysqli($servername, $username, $password, $database);

// Check the connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

// Fetch data from the "Damage" table
$sql = "SELECT * FROM Damage";
$result = $conn->query($sql);

if ($result) {
    // Initialize an array to store the fetched data
    $data = array();

    // Fetch data row by row
    while ($row = $result->fetch_assoc()) {
        $data[] = $row;
    }

    // Close the result set
    $result->close();

    // Return the data as JSON
    header('Content-Type: application/json');
    echo json_encode($data);
} else {
    echo "Error fetching data: " . $conn->error;
}

// Close the database connection
$conn->close();
?>
