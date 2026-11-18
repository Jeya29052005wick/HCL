"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
// 5. Function to display student details
function displayStudent(student) {
    console.log("Student ID:", student.id);
    console.log("Name:", student.name);
    console.log("Age:", student.age);
    console.log("Department:", student.department);
    console.log("Email:", student.email ?? "Not Provided");
    console.log("---------------------------");
}
// 6. Array containing 5 students
const students = [
    {
        id: 101,
        age: 21,
        name: "Jeya",
        department: "IT",
        email: "jeya@gmail.com"
    },
    {
        id: "S102",
        age: 20,
        name: "Arun",
        department: "CSE"
    },
    {
        id: 103,
        age: 22,
        name: "Kumar",
        department: "ECE",
        email: "kumar@gmail.com"
    },
    {
        id: "S104",
        age: 21,
        name: "Ravi",
        department: "EEE"
    },
    {
        id: 105,
        age: 20,
        name: "Priya",
        department: "IT",
        email: "priya@gmail.com"
    }
];
// 7. Display all student details
function displayAllStudents(studentList) {
    studentList.forEach((student) => {
        displayStudent(student);
    });
}
// 8. Calling the function
displayAllStudents(students);
// 9. Intersection Type Object
const studentContact = {
    id: 106,
    age: 21,
    name: "Vijay",
    department: "CSE",
    email: "vijay@gmail.com",
    phone: "9876543210",
    address: "Chennai"
};
console.log("Student with Contact Details:");
displayStudent(studentContact);
console.log("Phone:", studentContact.phone);
console.log("Address:", studentContact.address);
