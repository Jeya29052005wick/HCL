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
`
-- 20.	Sort students by DepartmentId ascending, then Age descending.
SELECT Name, DepartmentId,Age
FROM Students
ORDER BY DepartmentId,Age DESC;
      
-- 21.	Display each distinct age value.
-- 22.	Display distinct non-NULL department IDs.
-- 23.	Find names containing a digit using a LIKE pattern.
-- 24.	Find names containing any uppercase letter from A through M at the beginning.
-- 25.	Find students admitted during calendar year 2025.

