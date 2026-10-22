// This URL points to your existing ASP.NET Core controller.
const API_URL = "/api/Students";

// Get HTML elements so JavaScript can use them.
const studentTable = document.getElementById("studentTable");
const studentForm = document.getElementById("studentForm");
const formSection = document.getElementById("formSection");
const message = document.getElementById("message");

// Display a message at the top of the page.
function showMessage(text, isError = false) {
    message.textContent = text;
    message.classList.toggle("error", isError);
}

// Send a request to the API and report HTTP errors.
async function requestApi(url, options = {}) {
    const response = await fetch(url, {
        ...options,
        headers: {
            ...(options.body ? { "Content-Type": "application/json" } : {}),
            ...options.headers
        }
    });

    if (response.status === 204) {
        return null; // Successful update or delete; no response body.
    }

    const data = await response.json().catch(() => null);

    if (!response.ok) {
        throw new Error(    
            data?.message ||
            data?.title ||
            (response.status === 409
                ? "That email address already exists."
                : `Request failed with status ${response.status}.`)
        );
    }

    return data;
}

// Load all students and display them in the table.
async function loadStudents() {
    studentTable.innerHTML =
        '<tr><td colspan="6" class="empty">Loading students...</td></tr>';

    try {
        const students = await requestApi(API_URL);
        displayStudents(students);
    } catch (error) {
        studentTable.innerHTML =
            '<tr><td colspan="6" class="empty">Could not load students.</td></tr>';
        showMessage(error.message + " Check that the API is running.", true);
    }
}

// Create table rows from the student data returned by the API.
function displayStudents(students) {
    if (!students || students.length === 0) {
        studentTable.innerHTML =
            '<tr><td colspan="6" class="empty">No students found.</td></tr>';
        return;
    }

    studentTable.innerHTML = "";

    students.forEach(student => {
        const row = document.createElement("tr");

        // Use textContent instead of inserting student values as HTML.
        const values = [
            student.id,
            student.name,
            student.email,
            student.department,
            student.age
        ];

        values.forEach(value => {
            const cell = document.createElement("td");
            cell.textContent = value ?? "";
            row.appendChild(cell);
        });

        const actions = document.createElement("td");

        const viewButton = makeButton("View", "action-button", () => viewStudent(student.id));
        const editButton = makeButton("Edit", "action-button", () => editStudent(student));
        const deleteButton = makeButton("Delete", "action-button delete-button", () => deleteStudent(student.id));

        actions.append(viewButton, editButton, deleteButton);
        row.appendChild(actions);
        studentTable.appendChild(row);
    });
}

// Helper to create action buttons.
function makeButton(text, className, action) {
    const button = document.createElement("button");
    button.type = "button";
    button.textContent = text;
    button.className = className;
    button.addEventListener("click", action);
    return button;
}

// Show the form for adding a new student.
document.getElementById("showFormButton").addEventListener("click", () => {
    studentForm.reset();
    document.getElementById("studentId").value = "";
    document.getElementById("formTitle").textContent = "Add Student";
    document.getElementById("saveButton").textContent = "Save Student";
    formSection.hidden = false;
    showMessage("");
});

// Hide the form when Cancel is clicked.
document.getElementById("cancelButton").addEventListener("click", () => {
    formSection.hidden = true;
});

// Save a new student or update an existing student.
studentForm.addEventListener("submit", async event => {
    event.preventDefault(); // Prevent the browser from refreshing the page.

    const id = document.getElementById("studentId").value;
    const name = document.getElementById("name").value.trim();
    const email = document.getElementById("email").value.trim();
    const department = document.getElementById("department").value.trim();
    const age = Number(document.getElementById("age").value);

    // Basic validation before sending data to the API.
    if (!name || !email || !department) {
        showMessage("Please fill in every field.", true);
        return;
    }

    if (!Number.isInteger(age) || age < 17 || age > 60) {
        showMessage("Age must be a whole number between 17 and 60.", true);
        return;
    }

    const student = { name, email, department, age };
    let url = API_URL;
    let method = "POST";

    // When an ID exists, this form is editing a student.
    if (id) {
        student.id = Number(id);
        url = `${API_URL}/${id}`;
        method = "PUT";
    }

    try {
        await requestApi(url, {
            method: method,
            body: JSON.stringify(student)
        });

        formSection.hidden = true;
        studentForm.reset();
        showMessage(id ? "Student updated successfully." : "Student added successfully.");
        await loadStudents();
    } catch (error) {
        showMessage(error.message, true);
    }
});

// Search students by name, department, or both.
document.getElementById("searchForm").addEventListener("submit", async event => {
    event.preventDefault();

    const name = document.getElementById("searchName").value.trim();
    const department = document.getElementById("searchDepartment").value.trim();

    const params = new URLSearchParams();
    if (name) params.set("name", name);
    if (department) params.set("department", department);

    const url = params.toString()
        ? `${API_URL}/search?${params.toString()}`
        : API_URL;

    try {
        const students = await requestApi(url);
        displayStudents(students);
        showMessage(`${students.length} student(s) found.`);
    } catch (error) {
        showMessage(error.message, true);
    }
});

// Clear the search fields and load all students again.
document.getElementById("clearButton").addEventListener("click", () => {
    document.getElementById("searchName").value = "";
    document.getElementById("searchDepartment").value = "";
    showMessage("");
    loadStudents();
});

// View one student using GET /api/Students/{id}.
async function viewStudent(id) {
    try {
        const student = await requestApi(`${API_URL}/${id}`);
        alert(
            `Student ID: ${student.id}\n` +
            `Name: ${student.name}\n` +
            `Email: ${student.email}\n` +
            `Department: ${student.department}\n` +
            `Age: ${student.age}`
        );
    } catch (error) {
        showMessage(error.message, true);
    }
}

// Fill the form with existing values so the student can be edited.
function editStudent(student) {
    document.getElementById("studentId").value = student.id;
    document.getElementById("name").value = student.name;
    document.getElementById("email").value = student.email;
    document.getElementById("department").value = student.department;
    document.getElementById("age").value = student.age;

    document.getElementById("formTitle").textContent = "Edit Student";
    document.getElementById("saveButton").textContent = "Update Student";
    formSection.hidden = false;
    showMessage("");
    formSection.scrollIntoView({ behavior: "smooth" });
}

// Delete a student after asking the user to confirm.
async function deleteStudent(id) {
    if (!confirm("Are you sure you want to delete student ID " + id + "?")) {
        return;
    }

    try {
        await requestApi(`${API_URL}/${id}`, { method: "DELETE" });
        showMessage("Student deleted successfully.");
        await loadStudents();
    } catch (error) {
        showMessage(error.message, true);
    }
}

// Load student records when the page first opens.
loadStudents();
