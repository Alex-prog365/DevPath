using DevPath.Data;
using DevPath.Models;
using DevPath.Services;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace DevPath
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            InitializeDatabase();

            LoadCourses();
        }

        private void InitializeDatabase()
        {
            using var db = new DevPathDbContext();
            db.Database.EnsureCreated();

            if (!db.Courses.Any())
            {
                var course = new Course
                {
                    Title = "C# Basics",
                    Description = "Base course",
                    Progress = 0
                };

                db.Courses.Add(course);
                db.SaveChanges();

                CourseSeeder.CreateDefaultContent(db, course);
            }
        }


        private void LoadCourses() 
        {
            using var db = new DevPathDbContext();
            var courses = db.Courses.ToList();
                       
            CoursesListBox.ItemsSource = courses;
           
        }

        
        private void CoursesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CoursesListBox.SelectedItem is Course selectedCourse)
            {
                using var db = new DevPathDbContext();

                var lessons = db.Lessons.Where(l => l.CourseId == selectedCourse.Id).ToList();
                

                foreach (var lesson in lessons)
                {
                    lesson.Status = GetLessonStatus(lesson.Id);
                }

                LessonsListBox.ItemsSource = lessons;
                LessonsDescriptionTextBlock.Text = "";
                TopicsListBox.ItemsSource = null;
               
            }
        }



        private void LessonsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) 
        {

            if (LessonsListBox.SelectedItem is Lesson selectedLesson) 
            {
                TopicsListBox.DisplayMemberPath = "DisplayText";

                using var db = new DevPathDbContext();
                var sections = db.Topics.Where(s => s.LessonId == selectedLesson.Id).ToList();
                               
                TopicsListBox.ItemsSource = sections;
                TopicsListBox.DisplayMemberPath = "DisplayText";
               
            }

        }



        private void TopicsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TopicsListBox.SelectedItem is DevPath.Models.Topic selectedTopic)
            {
                var content = ContentService.Load(selectedTopic.ContentFile ?? "");
                ContentRenderer.Render(ExamplesPanel, content);
                               
                using var db = new DevPathDbContext();

                var section = db.Topics.FirstOrDefault(s => s.Id == selectedTopic.Id);
                if (section != null)
                {
                    section.IsCompleted = true;
                    db.SaveChanges();
                    selectedTopic.IsCompleted = true;
                    TopicsListBox.Items.Refresh();
                }

                if (LessonsListBox.SelectedItem is Lesson selectedLesson)
                {
                    selectedLesson.Status = GetLessonStatus(selectedLesson.Id);
                    LessonsListBox.Items.Refresh();

                    UpdateCourseProgress(selectedLesson.CourseId);

                    if (CoursesListBox.SelectedItem is Course selectedCourse)
                    {
                        var updatedProgress = db.Courses.First(c => c.Id == selectedCourse.Id).Progress;
                        selectedCourse.Progress = updatedProgress;
                        CoursesListBox.Items.Refresh();
                    }
                }
            }
        }


        private void UpdateCourseProgress(int courseId) 
        {
            using var db = new DevPathDbContext();

            var totalSections = db.Topics.Count(s => db.Lessons.Any(l => l.Id == s.LessonId && l.CourseId == courseId));

            var completedSections = db.Topics.Count(s => s.IsCompleted && db.Lessons.Any(l => l.Id == s.LessonId && l.CourseId == courseId));

            var course = db.Courses.FirstOrDefault(c => c.Id == courseId);

            if (course != null && totalSections > 0)
            {
                course.Progress = completedSections * 100 / totalSections;
                db.SaveChanges();
            }
        }

        
        private string GetLessonStatus(int lessonId) 
        {
            using var db = new DevPathDbContext();

            int totalSections = db.Topics.Count(s => s.LessonId == lessonId);
            int completedSections = db.Topics.Count(s => s.LessonId == lessonId && s.IsCompleted);

            if (completedSections == 0)
                return "Not started";

            if (completedSections == totalSections)
                return "Done";

            return "In progress";
            
        }



         
    }
}