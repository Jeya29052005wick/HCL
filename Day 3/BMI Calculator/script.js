const form = document.getElementById("bmiForm");

form.addEventListener("submit", function(event) {

    event.preventDefault();

    // Get input values
    const weight = Number(
        document.getElementById("weight").value
    );

    const height = Number(
        document.getElementById("height").value
    );

    // Validate inputs
    if (weight <= 0 || height <= 0) {
        alert("Please enter valid height and weight.");
        return;
    }

    // Convert height from cm to metres
    const heightInMetres = height / 100;

    // BMI formula
    const bmi = weight / (heightInMetres * heightInMetres);

    // Determine BMI category
    let category;

    if (bmi < 18.5) {
        category = "Underweight";
    } else if (bmi < 25) {
        category = "Normal weight";
    } else if (bmi < 30) {
        category = "Overweight";
    } else {
        category = "Obesity";
    }

    // Display result
    document.getElementById("bmiValue").textContent =
        bmi.toFixed(2);

    document.getElementById("bmiCategory").textContent =
        category;
});

