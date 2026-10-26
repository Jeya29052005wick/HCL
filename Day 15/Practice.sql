-- Topic 1 — Filtering and Sorting

-- 1.	Display the StudentId, Name, Email, and Age of all students.
SELECT StudentId, Name, Email, Age
FROM Students;

-- 2.	Find students who are exactly 21 years old.
SELECT Name
FROM Students
WHERE Age = 21;

-- 3.	Find students younger than 20.
SELECT Name
FROM Students
WHERE Age < 20;

-- 4.	Find active students aged 20 or above.
SELECT Name, IsActive,Age
FROM Students
Where IsActive = 1
AND Age >= 20;

-- 5.	Find students aged between 18 and 25 inclusive.
SELECT Name,Age
FROM Students
WHERE Age >= 18
AND Age <= 25;

-- 6.	Find students belonging to departments 1, 3, or 6.
SELECT Name, DepartmentId
FROM Students
WHERE DepartmentId IN (1,3,6);

-- 7.	Find students who have not been assigned a department.
SELECT Name, DepartmentId
FROM Students 
WHERE DepartmentId IS NULL;

-- 8.	Find students who have been assigned a department.
SELECT Name, DepartmentId
FROM Students 
WHERE DepartmentId IS NOT NULL;

-- 9.	Find names that begin with A.
SELECT Name
FROM Students
WHERE Name LIKE 'A%';

-- 10.	Find names that end with the digit 5.
SELECT Name
FROM Students
WHERE Name LIKE '%5';

-- 11.	Find names containing the text 'ar' (use case-insensitive matching where supported by the database collation).
SELECT Name
FROM Students
WHERE Name LIKE '%ar%';

-- 12.	Find names whose second character is 'a'.
SELECT Name
FROM Students
WHERE Name LIKE '_a%';

-- 13.	Find names that contain exactly one character before ' ' and then any text.
SELECT Name
FROM Students
WHERE Name LIKE '_ %';

-- 14.	Find emails ending in example.com.
SELECT Name, Email
FROM Students
WHERE Email LIKE '%example.com';

-- 15.	Find students whose age is not 20.
SELECT Name,Age
FROM Students
WHERE Age != 20;

-- 16.	Find students aged 18–22 who are active.
SELECT Name,Age,IsActive
FROM Students
WHERE Age BETWEEN 18 AND 22
      AND IsActive = 1;

-- 17.	Find students who are inactive or are not assigned to a department.
SELECT Name,IsActive,DepartmentId
FROM Students
WHERE IsActive = 0
	OR DepartmentId IS NULL;

-- 18.	Sort students from youngest to oldest.
SELECT Name,Age
FROM Students
ORDER BY Age;

-- 19.	Display the 10 oldest students.
SELECT TOP 10 Name, Age
FROM Students
ORDER BY Age DESC;

-- 20.	Sort students by DepartmentId ascending, then Age descending.
SELECT Name, DepartmentId,Age
FROM Students
ORDER BY DepartmentId,Age DESC;

-- 21. Display each distinct age value.
SELECT DISTINCT Age
FROM Students;

-- 22.	Display distinct non-NULL department IDs.
SELECT DISTINCT DepartmentID
FROM Students
WHERE DepartmentId is NOT NULL;

-- 23.	Find names containing a digit using a LIKE pattern.
SELECT Name 
FROM Students
WHERE Name LIKE '%[0-9]%';

-- 24.	Find names containing any uppercase letter from A through M at the beginning.
SELECT Name 
FROM Students
WHERE Name LIKE '[A-M]%';

-- 25.	Find students admitted during calendar year 2025.
SELECT * FROM Students 
WHERE AdmissionDate >= '20250101'
	  AND
	  AdmissionDate < '20260101';

-- Topic 2 — Aggregate Functions

-- 26.	Count all students.
SELECT Count(*) AS TotalStudents
FROM Students;

-- 27.	Count students who have an email value.
SELECT COUNT(Email) as StudentswithEmail
FROM Students;

-- 28.	Count students who have a department assigned.
SELECT COUNT(DepartmentID) AS StudentwithDEpt
FROM Students;

-- 29.	Find the youngest student age.
SELECT MIN(Age) As YoungAge
FROM Students;

-- 30.	Find the oldest student age.
SELECT MAX(Age) As YoungAge
FROM Students;

-- 31.	Calculate the average student age with two decimal places.
SELECT CAST(AVG(CAST(Age AS Decimal(10,2))) AS Decimal(10,2))
	   AS AverageAge
	   FROM Students;
		
-- 32.	Find the total number of credits across all courses.
SELECT SUM(Credits) AS TotalCredits
FROM Courses;

-- 33.	Find the average course fee.
SELECT AVG(Fee) As AvgCourseFee
FROM Courses;

-- 34.	Find the lowest and highest course fees.
SELECT MIN(Fee) As LowestFee, MAX(Fee) as HighestFee
FROM Courses;

-- 35.	Count distinct student ages.
SELECT COUNT(DISTINCT Age) as DistinctStudentAge
FROM Students;

-- 36.	Count distinct non-NULL department IDs represented in Students.
SELECT COUNT(DISTINCT DepartmentId) as DistinctDeptID
FROM Students
WHERE DepartmentId IS NOT NULL; -- NO NEED Distinct Not includes NULL

SELECT COUNT(DISTINCT DepartmentId) AS DistinctDeptID
FROM Students;

-- 37.	Count all enrollment records.
SELECT COUNT(*) As TotalEnrollment
FROM Enrollments;

-- 38.	Count completed enrollments.
SELECT COUNT(EnrollmentStatus) As EnrollStatus
FROM Enrollments
WHERE EnrollmentStatus = 'Completed';

-- 39.	Calculate the total amount of all payments.
SELECT SUM(Amount) AS TotalAmount
FROM Payments;

-- 40.	Calculate the total amount of payments with status Paid.
SELECT SUM(Amount) AS TotalAmount
FROM Payments
WHERE PaymentStatus = 'Paid';

-- 41.	Find the average marks obtained across non-NULL assessment results.
SELECT AVG(MarksObtained) AS AverageMarks
FROM AssessmentResults;

-- 42.	Find the highest mark obtained.
SELECT MAX(MarksObtained) AS HighestMark
FROM AssessmentResults;

-- 43.	Find the lowest non-NULL mark obtained.
SELECT MIN(MarksObtained) AS MaxMark
FROM AssessmentResults 
WHERE MarksObtained IS NOT NULL;

-- 44.	Count assessment results with no marks.
SELECT COUNT(*) AS NoMarks
FROM AssessmentResults 
WHERE MarksObtained IS NULL;

-- 45.	Count submitted results.
SELECT COUNT(*) AS Result
FROM AssessmentResults
WHERE SubmissionStatus = 'Submitted';

Also COUNT(SubmissionStatus); -- works

-- 46.	Calculate the average marks for submitted results only.
SELECT AVG(MarksObtained) AS MarksForSubmittedResults
FROM AssessmentResults
WHERE SubmissionStatus = 'Submitted';

-- 47.	Find the total number of assessments.
SELECT COUNT(*) AS TotalAssessments
FROM Assessments;

-- 48.	Find the total maximum marks across all assessments.
SELECT SUM(MaxMarks) AS TotalMaxMarks
FROM Assessments;

-- 49.	Find the most recent payment date.
SELECT  MAX(PaymentDate) AS MostRecentPaymentDate
FROM Payments;

-- 50.	Find the total number of active courses.
SELECT COUNT(*) AS ActiveCourses
FROM Courses
WHERE IsActive = 1;

-- Topic 3 — GROUP BY and HAVING

-- 51.	Count students in each department ID, including students with NULL DepartmentId as a group.
SELECT DepartmentId,COUNT(*) AS TotalStudents
FROM Students
GROUP BY DepartmentId;

-- 52.	Find the average age for each department ID.
SELECT DepartmentId,AVG(CAST(Age AS Decimal(10,2))) As AverageAge
FROM Students
GROUP BY DepartmentId;

-- 53.	Find the youngest and oldest student age per department.
SELECT DepartmentId,
	   MIN(Age) AS LowestAge,
	   MAX(Age) AS HighestAge
	   FROM Students
	   GROUP BY DepartmentId;

-- 54.	Show department IDs that have more than 10 students.
SELECT DepartmentId,
	   COUNT(StudentId) AS TotalStudents
	   FROM Students
	   GROUP BY DepartmentId
	   HAVING COUNT(*) > 10;

-- 55.	Count students by active status.
SELECT IsActive,COUNT(*) AS TotalStudents
FROM Students
GROUP BY IsActive;

-- 56.	Count enrollments by EnrollmentStatus.
SELECT EnrollmentStatus,COUNT(*) AS TotalEnrollments
FROM Enrollments
GROUP BY EnrollmentStatus;

-- 57.	Find courses with more than 15 enrollments.
SELECT CourseId, COUNT(*) AS TotalEnrollments
FROM Enrollments
GROUP BY CourseId
HAVING COUNT(*) > 15;

-- 58.	Find the average course fee per category.
SELECT Category,AVG(Fee) AS AverageCourseFee
FROM Courses 
GROUP BY Category;

-- 59.	Count courses per category.
SELECT Category,COUNT(*) AS TotalCourses
FROM Courses
GROUP BY Category;

-- 60.	Show payment count and total amount by payment status.
SELECT PaymentStatus, COUNT(*) AS PaymentCount, SUM(Amount) AS TotalAmount
FROM Payments
GROUP BY PaymentStatus;

-- 61.	Show payment count and average amount by payment method.
SELECT PaymentMethod, AVG(Amount) AS AverageAmount, COUNT(*) AS PaymentCount
FROM Payments
GROUP BY PaymentMethod;

-- 62.	Find payment statuses with at least 20 payments.
SELECT PaymentStatus,COUNT(*) AS PaymentCount
FROM Payments
GROUP BY PaymentStatus
HAVING COUNT(*) >= 20;

-- 63.	Find students with more than 3 enrollments.
SELECT StudentId,COUNT(*) AS TotalEnrollments
FROM Enrollments
GROUP BY StudentId
HAVING COUNT(*) > 3;

-- 64. Find students with at least 2 assessment results.
SELECT StudentId, COUNT(*) AS TotalAssessmentResults
FROM AssessmentResults
GROUP BY StudentId
HAVING COUNT(*) >= 2;

-- 65. Find average marks per assessment.
SELECT AssessmentId, AVG(MarksObtained) AS AverageMarks
FROM AssessmentResults
GROUP BY AssessmentId;

-- 66.	Find assessments whose average marks are above 60.
SELECT AssessmentId,AVG(MarksObtained) AS AverageMarks
FROM AssessmentResults
GROUP BY AssessmentId
HAVING AVG(MarksObtained) > 60;

-- 67.	Count result records by SubmissionStatus.
SELECT SubmissionStatus, COUNT(*) AS TotalSubmission
FROM AssessmentResults
GROUP BY SubmissionStatus;

-- 68.	Find the total fee per course category for active courses.
SELECT Category, SUM(Fee) AS TotalFees
FROM Courses
WHERE IsActive = 1
GROUP BY Category;

-- 69.	Find departments with average student age above 22.
SELECT DepartmentId,AVG(Age) AS AverageAge
FROM Students
GROUP BY DepartmentId
HAVING AVG(Age) > 22;

-- 70. Count students by DepartmentId and IsActive.
SELECT DepartmentId, IsActive, COUNT(StudentId) AS TotalStudents
FROM Students
GROUP BY DepartmentId, IsActive;

-- 71. Find courses with at least 2 assessments.
SELECT CourseId, COUNT(*) AS TotalAssessments
FROM Assessments
GROUP BY CourseId
HAVING COUNT(*) >= 2;

-- 72.	Find each students total payment amount.
SELECT StudentId,SUM(Amount) AS TotalAmount
FROM Payments
GROUP BY StudentId

-- 73.	Find students whose total payments exceed 12000.
SELECT StudentId,SUM(Amount) AS TotalAmount
FROM Payments
GROUP BY StudentId
HAVING SUM(Amount) > 12000;

-- 74. Count results with non-NULL marks per student.
SELECT StudentId, COUNT(MarksObtained) AS ResultsWithMarks
FROM AssessmentResults
WHERE MarksObtained IS NOT NULL
GROUP BY StudentId;

-- 75. Find payment months with more than 10 payments.
SELECT YEAR(PaymentDate) AS PaymentYear, MONTH(PaymentDate) AS PaymentMonth, COUNT(*) AS TotalPayments
FROM Payments
GROUP BY YEAR(PaymentDate), MONTH(PaymentDate)
HAVING COUNT(*) > 10;




