using System.Collections.Generic;
using System.Linq;
using DevPath.Models;

namespace DevPath.Data
{
    public static class CourseSeeder
    {
        public static void CreateDefaultContent(DevPathDbContext db, Course course)
        {
            AddLessonWithTopics(db, course, "Introduction", CreateIntroductionTopics());
            AddLessonWithTopics(db, course, "1. Variables", CreateVariableTopics());
            AddLessonWithTopics(db, course, "2. Input / Output", CreateInputOutputTopics());
            AddLessonWithTopics(db, course, "3. Conditions", CreateConditionTopics());
            AddLessonWithTopics(db, course, "4. Operators", CreateOperatorTopics());
            AddLessonWithTopics(db, course, "5. Loops", CreateLoopTopics());
            AddLessonWithTopics(db, course, "6. Methods", CreateMethodTopics());
            AddLessonWithTopics(db, course, "7. Arrays / Lists", CreateArrayListTopics());
            AddLessonWithTopics(db, course, "8. Classes", CreateClassTopics());
            AddLessonWithTopics(db, course, "9. Final Project", CreateFinalProjectTopics());
        }

        
        private static void AddLessonWithTopics(DevPathDbContext db, Course course, string lessonTitle, List<(string Title, string ContentFile)> topics)
        {
            var lesson = new Lesson
            {
                CourseId = course.Id,
                Title = lessonTitle,
                Status = "Not started"
            };

            db.Lessons.Add(lesson);
            db.SaveChanges();

            AddTopics(db, lesson, topics);
            db.SaveChanges();
        }


        private static void AddTopics(DevPathDbContext db, Lesson lesson, List<(string Title, string ContentFile)> topics)
        {
            foreach (var topic in topics)
            {
                db.Topics.Add(new Topic
                {
                    LessonId = lesson.Id,
                    Title = topic.Title,
                    ContentFile = topic.ContentFile,
                    IsCompleted = false
                });
            }
        }


        private static List<(string Title, string ContentFile)> CreateIntroductionTopics()
        {
            return new List<(string, string)>
            {
                ("What is programming", "csharp_basic/00_introbuction/introduction_what_is_programming.json"),
                ("How programs work", "csharp_basic/00_introbuction/introduction_how_programs_work.json"),
                ("What is C#", "csharp_basic/00_introbuction/introduction_what_is_csharp.json"),
                ("How this course works", "csharp_basic/00_introbuction/introduction_how_this_course_works.json"),
                ("Your first simple program", "csharp_basic/00_introbuction/introduction_your_first_simple_program.json")
            };
    
        }

        private static List<(string Title, string ContentFile)> CreateVariableTopics()
        {
            return new List<(string, string)>
            {
                ("What is a variable", "csharp_basic/01_variables/variables_what_is_a_variable.json"),
                ("How to declare a variable", "csharp_basic/01_variables/variables_how_to_declare_a_variable.json"),
                ("Assigning a value", "csharp_basic/01_variables/variables_assigning_a_value.json"),
                ("Changing a variable value", "csharp_basic/01_variables/variables_changing_a_variable_value.json"),
                ("Common data types", "csharp_basic/01_variables/variables_common_data_types.json"),
                ("var keyword", "csharp_basic/01_variables/variables_var_keyword.json"),
                ("Constants", "csharp_basic/01_variables/variables_constants.json"),
                ("Naming rules for variables", "csharp_basic/01_variables/variables_naming_rules_for_variables.json"),
                ("Simple practice with variables", "csharp_basic/01_variables/variables_simple_practice_with_variables.json")
            };
        }


        private static List<(string Title, string ContentFile)> CreateInputOutputTopics()
        {
            return new List<(string, string)>
            {
                ("Console output with WriteLine", "csharp_basic/02_input_output/input_output_console_output_writeline.json"),
                ("Printing variables to console", "csharp_basic/02_input_output/input_output_printing_variables.json"),
                ("Console input with ReadLine", "csharp_basic/02_input_output/input_output_console_input_readline.json"),
                ("Converting text to numbers", "csharp_basic/02_input_output/input_output_converting_text_to_numbers.json"),
                ("Simple input and output practice", "csharp_basic/02_input_output/input_output_simple_practice.json")
            };
        }


        private static List<(string Title, string ContentFile)> CreateConditionTopics()
        {
            return new List<(string, string)>
            {
                ("What is a condition", "csharp_basic/03_conditions/conditions_what_is_a_condition.json"),
                ("The if statement", "csharp_basic/03_conditions/conditions_if_statement.json"),
                ("The else statement", "csharp_basic/03_conditions/conditions_else_statement.json"),
                ("else if chains", "csharp_basic/03_conditions/conditions_else_if_chains.json"),
                ("Simple decision program", "csharp_basic/03_conditions/conditions_simple_decision_program.json")
            };
        }


        private static List<(string Title, string ContentFile)> CreateOperatorTopics()
        {
            return new List<(string, string)>
            {
                ("What is an operator", "csharp_basic/04_operators/operators_what_is_an_operator.json"),
                ("Arithmetic operators", "csharp_basic/04_operators/operators_arithmetic_operators.json"),
                ("Comparison operators", "csharp_basic/04_operators/operators_comparison_operators.json"),
                ("Logical operators", "csharp_basic/04_operators/operators_logical_operators.json"),
                ("Assignment operators", "csharp_basic/04_operators/operators_assignment_operators.json"),
                ("Increment and decrement", "csharp_basic/04_operators/operators_increment_decrement.json"),
                ("Operator precedence", "csharp_basic/04_operators/operators_operator_precedence.json"),
                ("Simple calculations practice", "csharp_basic/04_operators/operators_simple_calculations_practice.json")
            };
        }


        private static List<(string Title, string ContentFile)> CreateLoopTopics()
        {
            return new List<(string, string)>
            {
                ("What is a loop", "csharp_basic/05_loops/loops_what_is_a_loop.json"),
                ("The while loop", "csharp_basic/05_loops/loops_while_loop.json"),
                ("The for loop", "csharp_basic/05_loops/loops_for_loop.json"),
                ("Loop counters", "csharp_basic/05_loops/loops_loop_counters.json"),
                ("Break and continue", "csharp_basic/05_loops/loops_break_continue.json"),
                ("Simple loop practice", "csharp_basic/05_loops/loops_simple_loop_practice.json")
            };
        }

        private static List<(string Title, string ContentFile)> CreateMethodTopics()
        {
            return new List<(string, string)>
            {
                ("What is a method", "csharp_basic/06_methods/methods_what_is_a_method.json"),
                ("Creating your first method", "csharp_basic/06_methods/methods_creating_first_method.json"),
                ("Calling a method", "csharp_basic/06_methods/methods_calling_a_method.json"),
                ("Method parameters", "csharp_basic/06_methods/methods_method_parameters.json"),
                ("Return values", "csharp_basic/06_methods/methods_return_values.json"),
                ("Splitting program into methods", "csharp_basic/06_methods/methods_splitting_program_into_methods.json"),
                ("Simple methods practice", "csharp_basic/06_methods/methods_simple_methods_practice.json")
            };
        }

        private static List<(string Title, string ContentFile)> CreateArrayListTopics()
        {
            return new List<(string, string)>
            {
                ("What is an array", "csharp_basic/07_arrays/arrays_what_is_an_array.json"),
                ("Creating an array", "csharp_basic/07_arrays/arrays_creating_an_array.json"),
                ("Accessing array elements", "csharp_basic/07_arrays/arrays_accessing_array_elements.json"),
                ("Looping through arrays", "csharp_basic/07_arrays/arrays_looping_through_arrays.json"),
                ("What is a list", "csharp_basic/07_arrays/lists_what_is_a_list.json"),
                ("Creating and using lists", "csharp_basic/07_arrays/lists_creating_using_lists.json"),
                ("Simple collections practice", "csharp_basic/07_arrays/collections_simple_practice.json")
            };
        }

        private static List<(string Title, string ContentFile)> CreateClassTopics()
        {
            return new List<(string, string)>
            {
                ("What is a class", "csharp_basic/08_classes/classes_what_is_a_class.json"),
                ("Creating a simple class", "csharp_basic/08_classes/classes_creating_simple_class.json"),
                ("Fields and properties", "csharp_basic/08_classes/classes_fields_and_properties.json"),
                ("Methods inside a class", "csharp_basic/08_classes/classes_methods_inside_class.json"),
                ("Creating objects", "csharp_basic/classes/08_classes_creating_objects.json"),
                ("Using objects in a program", "csharp_basic/08_classes/classes_using_objects_in_program.json"),
                ("Simple class practice", "csharp_basic/08_classes/classes_simple_practice.json")
            };
        }


        private static List<(string Title, string ContentFile)> CreateFinalProjectTopics()
        {
            return new List<(string, string)>
            {
                ("Project idea and structure", "csharp_basic/09_project/project_idea_structure.json"),
                ("Create the base program (menu and input)", "csharp_basic/09_project/project_create_base_program.json"),
                ("Add the program loop", "csharp_basic/09_project/project_add_program_loop.json"),
                ("Add the task list (collections)", "csharp_basic/09_project/project_add_task_list.json"),
                ("Split the program into methods", "csharp_basic/09_project/project_split_into_methods.json"),
                ("Create a simple class", "csharp_basic/09_project/project_create_simple_class.json"),
                ("Final run and testing", "csharp_basic/09_project/project_final_run_testing.json")
            };
        }

    }
}
