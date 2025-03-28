using System;

class Program
{
    static void Main()
    {
        // Define the 2D array (matrix) to store grades of 5 students in 3 subjects
        int[,] grades = {
            { 85, 92, 88 },  // Student 1
            { 78, 82, 84 },  // Student 2
            { 92, 90, 95 },  // Student 3
            { 70, 75, 80 },  // Student 4
            { 75, 85, 90 }   // Student 5
        };

        // Get the number of students (rows) and subjects (columns)
        int numStudents = grades.GetLength(0); // Get number of rows (students)
        int numSubjects = grades.GetLength(1); // Get number of columns (subjects)

        // 1. Display the grades matrix
        Console.WriteLine("Grades Matrix:");
        for (int i = 0; i < numStudents; i++)  // Loop through each student (row)
        {
            for (int j = 0; j < numSubjects; j++)  // Loop through each subject (column)
            {
                Console.Write(grades[i, j] + "\t"); // Print each grade with a tab space
            }
            Console.WriteLine(); // Move to the next line after printing all subjects for a student
        }
        Console.WriteLine(); // Print an extra blank line for spacing

        // 2. Calculate and display the average grades per student
        Console.WriteLine("Average Grades per Student:");
        for (int i = 0; i < numStudents; i++)  // Loop through each student
        {
            int sum = 0; // Variable to store the sum of grades for the current student

            for (int j = 0; j < numSubjects; j++)  // Loop through each subject for the student
            {
                sum += grades[i, j]; // Add the subject grade to the sum
            }

            double average = (double)sum / numSubjects; // Calculate the average
            Console.WriteLine($"Student {i + 1}: {average}"); // Display the student's average
        }
        Console.WriteLine(); // Print an extra blank line for spacing

        // 3. Find and display the highest grade per subject
        Console.WriteLine("Highest Grades per Subject:");
        for (int j = 0; j < numSubjects; j++)  // Loop through each subject
        {
            int highest = grades[0, j]; // Assume the first student's grade is the highest initially

            for (int i = 1; i < numStudents; i++) // Compare with other students' grades in the same subject
            {
                if (grades[i, j] > highest) // If we find a higher grade, update the highest value
                {
                    highest = grades[i, j];
                }
            }

            Console.WriteLine($"Subject {j + 1}: {highest}"); // Display the highest grade for the subject
        }

        Console.WriteLine("\nPress any key to close this window...");
        Console.ReadKey(); // Waits for user input before closing
    }
}
