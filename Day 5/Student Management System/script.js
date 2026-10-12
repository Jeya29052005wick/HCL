const studentForm = document.getElementById("studentForm");
const studentTable = document.getElementById("studentTable");
const totalStudents = document.getElementById("totalStudents");
const emptyMessage = document.getElementById("emptyMessage");

// Store student details
let students = [];

// Add student
studentForm.addEventListener("submit", function(event) {

    event.preventDefault();

    const student = {
        id: Date.now(),
        name: document.getElementById("name").value.trim(),
        phone: document.getElementById("phone").value.trim(),
        collegeId: document.getElementById("collegeId").value.trim(),
        email: document.getElementById("email").value.trim()
    };

    // Prevent duplicate college IDs
    const exists = students.some(function(item) {
        return item.collegeId.toLowerCase() ===
               student.collegeId.toLowerCase();
    });

    if (exists) {
        alert("This College ID already exists!");
        return;
    }

    // Store object inside array
    students.push(student);

    // Display updated list
    displayStudents();

    // Clear form inputs
    studentForm.reset();
});

// Display all students
function displayStudents() {

    studentTable.innerHTML = "";

    students.forEach(function(student) {

        const row = document.createElement("tr");

        // Create cells safely
        [
            student.name,
            student.phone,
            student.collegeId,
            student.email
        ].forEach(function(value) {

            const cell = document.createElement("td");
            cell.textContent = value;
            row.appendChild(cell);

        });

        // Create delete button
        const actionCell = document.createElement("td");
        const deleteButton = document.createElement("button");

        deleteButton.textContent = "Delete";
        deleteButton.className = "delete-btn";

        deleteButton.addEventListener("click", function() {
            deleteStudent(student.id);
        });

        actionCell.appendChild(deleteButton);
        row.appendChild(actionCell);

        studentTable.appendChild(row);
    });

    // Update student count
    totalStudents.textContent = students.length;

    // Show or hide empty message
    emptyMessage.style.display =
        students.length === 0 ? "block" : "none";
}

// Delete student
function deleteStudent(id) {

    students = students.filter(function(student) {
        return student.id !== id;
    });

    displayStudents();
}
