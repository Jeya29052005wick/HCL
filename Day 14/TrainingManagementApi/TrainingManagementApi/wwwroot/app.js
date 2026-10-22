
const API = {
    courses: "/api/courses",
    learners: "/api/learners",
    enrollments: "/api/enrollments"
};

const $ = (id) => document.getElementById(id);

let allCourses = [];
let allLearners = [];
let allEnrollments = [];
let messageTimer;

function showMessage(message, type = "success") {
    const box = $("message");

    box.textContent = message;
    box.className = `message ${type}`;
    box.hidden = false;

    clearTimeout(messageTimer);
    messageTimer = setTimeout(() => {
        box.hidden = true;
    }, 4000);
}

async function apiRequest(url, options = {}) {
    let response;

    try {
        response = await fetch(url, {
            ...options,
            headers: {
                ...(options.body
                    ? { "Content-Type": "application/json" }
                    : {}),
                ...options.headers
            }
        });
    } catch {
        throw new Error(
            "Cannot connect to the API. Check that the application is running."
        );
    }

    const data = response.status === 204
        ? null
        : await response.json().catch(() => null);

    if (!response.ok) {
        throw new Error(
            data?.message ||
            data?.title ||
            `Request failed with status ${response.status}.`
        );
    }

    return data;
}

function escapeHtml(value) {
    return String(value ?? "").replace(/[&<>"']/g, char => ({
        "&": "&amp;",
        "<": "&lt;",
        ">": "&gt;",
        '"': "&quot;",
        "'": "&#39;"
    })[char]);
}

function formatCurrency(value) {
    return new Intl.NumberFormat("en-IN", {
        style: "currency",
        currency: "INR",
        maximumFractionDigits: 2
    }).format(value);
}

function formatDate(value) {
    if (!value) return "—";

    const date = new Date(value);

    return Number.isNaN(date.getTime())
        ? "—"
        : date.toLocaleDateString("en-IN");
}

function emptyRow(columns, message) {
    return `<tr><td colspan="${columns}" class="empty">${escapeHtml(message)
        }</td></tr>`;
}

/* ---------------- Navigation ---------------- */

function navigate(page) {
    document.querySelectorAll(".page").forEach(section => {
        section.hidden = section.id !== `${page}-page`;
    });

    document.querySelectorAll(".nav-item").forEach(button => {
        button.classList.toggle(
            "active",
            button.dataset.page === page
        );
    });

    const titles = {
        dashboard: "Dashboard",
        courses: "Courses",
        learners: "Learners",
        enrollments: "Enrollments"
    };

    $("page-title").textContent = titles[page] || "Dashboard";

    if (page === "courses") loadCourses();
    if (page === "learners") loadLearners();
    if (page === "enrollments") loadEnrollments();
}

document.querySelectorAll(".nav-item").forEach(button => {
    button.addEventListener("click", () => {
        navigate(button.dataset.page);
    });
});

document.querySelectorAll("[data-go]").forEach(button => {
    button.addEventListener("click", () => {
        navigate(button.dataset.go);
    });
});

/* ---------------- Courses ---------------- */

async function loadCourses() {
    const search = $("course-search").value.trim();
    const sortBy = $("course-sort").value;

    const params = new URLSearchParams();

    if (search) params.set("search", search);
    if (sortBy) params.set("sortBy", sortBy);

    try {
        const query = params.toString();
        const courses = await apiRequest(
            `${API.courses}${query ? `?${query}` : ""}`
        );

        allCourses = courses;
        renderCourses(courses);
        renderDashboard();
        populateEnrollmentOptions();
    } catch (error) {
        $("courses-table").innerHTML = emptyRow(5, error.message);
        showMessage(error.message, "error");
    }
}

function renderCourses(courses) {
    $("courses-table").innerHTML = courses.length
        ? courses.map(course => `
            <tr>
                <td>
                    <span class="course-code">${escapeHtml(course.courseCode)
            }</span>
                    <span class="muted">${escapeHtml(course.title)
            }</span>
                </td>
                <td>${formatCurrency(course.fee)}</td>
                <td>${course.durationDays} days</td>
                <td>${course.totalSeats}</td>
                <td><span class="pill">${course.availableSeats
            } available</span></td>
            </tr>
        `).join("")
        : emptyRow(5, "No courses found.");
}

$("course-search").addEventListener("input", loadCourses);
$("course-sort").addEventListener("change", loadCourses);

$("show-course-form").addEventListener("click", () => {
    $("course-form").hidden = !$("course-form").hidden;
});

$("cancel-course-form").addEventListener("click", () => {
    $("course-form").reset();
    $("course-form").hidden = true;
});

$("course-form").addEventListener("submit", async event => {
    event.preventDefault();

    const course = {
        courseCode: $("course-code").value.trim(),
        title: $("course-title").value.trim(),
        fee: Number($("course-fee").value),
        durationDays: Number($("course-duration").value),
        totalSeats: Number($("course-seats").value)
    };

    try {
        await apiRequest(API.courses, {
            method: "POST",
            body: JSON.stringify(course)
        });

        $("course-form").reset();
        $("course-form").hidden = true;

        showMessage("Course added successfully.");
        await refreshAll();
        navigate("courses");
    } catch (error) {
        showMessage(error.message, "error");
    }
});

/* ---------------- Learners ---------------- */

async function loadLearners() {
    const search = $("learner-search").value.trim();
    const params = new URLSearchParams();

    if (search) params.set("search", search);

    try {
        const query = params.toString();
        const learners = await apiRequest(
            `${API.learners}${query ? `?${query}` : ""}`
        );

        allLearners = learners;
        renderLearners(learners);
        renderDashboard();
        populateEnrollmentOptions();
    } catch (error) {
        $("learners-table").innerHTML = emptyRow(4, error.message);
        showMessage(error.message, "error");
    }
}

function renderLearners(learners) {
    $("learners-table").innerHTML = learners.length
        ? learners.map(learner => `
            <tr>
                <td><span class="course-code">${escapeHtml(learner.learnerId)
            }</span></td>
                <td>${escapeHtml(learner.name)}</td>
                <td>${escapeHtml(learner.contactNumber)}</td>
                <td>${escapeHtml(learner.email)}</td>
            </tr>
        `).join("")
        : emptyRow(4, "No learners found.");
}

$("learner-search").addEventListener("input", loadLearners);

$("show-learner-form").addEventListener("click", () => {
    $("learner-form").hidden = !$("learner-form").hidden;
});

$("cancel-learner-form").addEventListener("click", () => {
    $("learner-form").reset();
    $("learner-form").hidden = true;
});

$("learner-form").addEventListener("submit", async event => {
    event.preventDefault();

    const learner = {
        learnerId: $("learner-id").value.trim(),
        name: $("learner-name").value.trim(),
        contactNumber: $("learner-contact").value.trim(),
        email: $("learner-email").value.trim()
    };

    try {
        await apiRequest(API.learners, {
            method: "POST",
            body: JSON.stringify(learner)
        });

        $("learner-form").reset();
        $("learner-form").hidden = true;

        showMessage("Learner registered successfully.");
        await refreshAll();
        navigate("learners");
    } catch (error) {
        showMessage(error.message, "error");
    }
});

/* ---------------- Enrollments ---------------- */

async function loadEnrollments() {
    try {
        allEnrollments = await apiRequest(API.enrollments);
        renderEnrollments(allEnrollments);
        renderDashboard();
    } catch (error) {
        $("enrollments-table").innerHTML = emptyRow(6, error.message);
        showMessage(error.message, "error");
    }
}

function renderEnrollments(enrollments) {
    $("enrollments-table").innerHTML = enrollments.length
        ? enrollments.map(enrollment => {
            const cancelled = enrollment.status === "Cancelled";

            return `
                <tr>
                    <td>${enrollment.enrollmentId}</td>
                    <td>
                        ${escapeHtml(enrollment.learnerName)}
                        <span class="muted">${escapeHtml(enrollment.learnerId)
                }</span>
                    </td>
                    <td>
                        ${escapeHtml(enrollment.courseTitle)}
                        <span class="muted">${escapeHtml(enrollment.courseCode)
                }</span>
                    </td>
                    <td>${formatDate(enrollment.enrollmentDate)}</td>
                    <td>
                        <span class="pill ${cancelled ? "cancelled" : ""
                }">${escapeHtml(enrollment.status)}</span>
                    </td>
                    <td>
                        ${cancelled
                    ? "—"
                    : `<button class="action-btn"
                                data-cancel="${enrollment.enrollmentId}">
                                Cancel
                              </button>`
                }
                    </td>
                </tr>
            `;
        }).join("")
        : emptyRow(6, "No enrollment records found.");
}

$("enrollments-table").addEventListener("click", async event => {
    const button = event.target.closest("[data-cancel]");
    if (!button) return;

    const id = button.dataset.cancel;

    if (!confirm(`Cancel enrollment #${id}?`)) return;

    try {
        const result = await apiRequest(
            `${API.enrollments}/${id}`,
            { method: "DELETE" }
        );

        showMessage(result.message || "Enrollment cancelled.");
        await refreshAll();
    } catch (error) {
        showMessage(error.message, "error");
    }
});

$("show-enrollment-form").addEventListener("click", async () => {
    try {
        await Promise.all([loadLearners(), loadCourses()]);
        populateEnrollmentOptions();

        $("enrollment-form").hidden =
            !$("enrollment-form").hidden;
    } catch (error) {
        showMessage(error.message, "error");
    }
});

$("cancel-enrollment-form").addEventListener("click", () => {
    $("enrollment-form").reset();
    $("enrollment-form").hidden = true;
});

function populateEnrollmentOptions() {
    const learnerSelect = $("enrollment-learner");
    const courseSelect = $("enrollment-course");

    const selectedLearner = learnerSelect.value;
    const selectedCourse = courseSelect.value;

    learnerSelect.innerHTML =
        '<option value="">Select learner</option>' +
        allLearners.map(learner => `
            <option value="${escapeHtml(learner.learnerId)}">
                ${escapeHtml(learner.learnerId)} — ${escapeHtml(learner.name)
            }
            </option>
        `).join("");

    courseSelect.innerHTML =
        '<option value="">Select course</option>' +
        allCourses.filter(course => course.availableSeats > 0)
            .map(course => `
                <option value="${escapeHtml(course.courseCode)}">
                    ${escapeHtml(course.courseCode)} — ${escapeHtml(course.title)
                } (${course.availableSeats} seats)
                </option>
            `).join("");

    learnerSelect.value = selectedLearner;
    courseSelect.value = selectedCourse;
}

$("enrollment-form").addEventListener("submit", async event => {
    event.preventDefault();

    const request = {
        learnerId: $("enrollment-learner").value,
        courseCode: $("enrollment-course").value
    };

    try {
        await apiRequest(API.enrollments, {
            method: "POST",
            body: JSON.stringify(request)
        });

        $("enrollment-form").reset();
        $("enrollment-form").hidden = true;

        showMessage("Learner enrolled successfully.");
        await refreshAll();
        navigate("enrollments");
    } catch (error) {
        showMessage(error.message, "error");
    }
});

/* ---------------- Dashboard ---------------- */

async function renderDashboard() {
    $("stat-courses").textContent = allCourses.length;
    $("stat-learners").textContent = allLearners.length;

    $("stat-enrollments").textContent = allEnrollments.filter(
        enrollment => enrollment.status === "Registered"
    ).length;

    $("stat-seats").textContent = allCourses.reduce(
        (total, course) => total + course.availableSeats,
        0
    );

    $("dashboard-courses").innerHTML = allCourses.length
        ? allCourses.slice(0, 5).map(course => `
            <tr>
                <td>
                    <span class="course-code">${escapeHtml(course.courseCode)
            }</span>
                    <span class="muted">${escapeHtml(course.title)
            }</span>
                </td>
                <td>${formatCurrency(course.fee)}</td>
                <td>${course.durationDays} days</td>
                <td>${course.availableSeats} / ${course.totalSeats}</td>
            </tr>
        `).join("")
        : emptyRow(4, "No courses available.");
}

async function refreshAll() {
    await Promise.all([
        loadCourses(),
        loadLearners(),
        loadEnrollments()
    ]);
}

/* ---------------- Start application ---------------- */

async function startApp() {
    try {
        await refreshAll();
    } catch (error) {
        showMessage(error.message, "error");
    }
}

startApp();
