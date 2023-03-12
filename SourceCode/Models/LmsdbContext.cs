using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SourceCode.Models;

public partial class LmsdbContext : DbContext
{
    public LmsdbContext()
    {
    }

    public LmsdbContext(DbContextOptions<LmsdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdminTbl> AdminTbls { get; set; }

    public virtual DbSet<AnswerTbl> AnswerTbls { get; set; }

    public virtual DbSet<AttendanceTbl> AttendanceTbls { get; set; }

    public virtual DbSet<CourseTbl> CourseTbls { get; set; }

    public virtual DbSet<EventTbl> EventTbls { get; set; }

    public virtual DbSet<ExamTbl> ExamTbls { get; set; }

    public virtual DbSet<FileTbl> FileTbls { get; set; }

    public virtual DbSet<LecturerTbl> LecturerTbls { get; set; }

    public virtual DbSet<NotificationTbl> NotificationTbls { get; set; }

    public virtual DbSet<QuestionTbl> QuestionTbls { get; set; }

    public virtual DbSet<RegisteredCoursesTbl> RegisteredCoursesTbls { get; set; }

    public virtual DbSet<ResultTbl> ResultTbls { get; set; }

    public virtual DbSet<StudentTbl> StudentTbls { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-MAT9FS5;Database=LMSDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdminTbl>(entity =>
        {
            entity.ToTable("Admin_tbl");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .HasColumnName("ID");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.NoNotifications).HasColumnName("No_Notifications");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Salt).HasMaxLength(50);
        });

        modelBuilder.Entity<AnswerTbl>(entity =>
        {
            entity.HasKey(e => e.AnsId);

            entity.ToTable("Answer_tbl");

            entity.Property(e => e.AnsId)
                .HasMaxLength(50)
                .HasColumnName("Ans_ID");
            entity.Property(e => e.AcheivedMark).HasColumnName("Acheived_Mark");
            entity.Property(e => e.Ans1).HasColumnName("Ans_1");
            entity.Property(e => e.Ans10).HasColumnName("Ans_10");
            entity.Property(e => e.Ans2).HasColumnName("Ans_2");
            entity.Property(e => e.Ans3).HasColumnName("Ans_3");
            entity.Property(e => e.Ans4).HasColumnName("Ans_4");
            entity.Property(e => e.Ans5).HasColumnName("Ans_5");
            entity.Property(e => e.Ans6).HasColumnName("Ans_6");
            entity.Property(e => e.Ans7).HasColumnName("Ans_7");
            entity.Property(e => e.Ans8).HasColumnName("Ans_8");
            entity.Property(e => e.Ans9).HasColumnName("Ans_9");
            entity.Property(e => e.ExamId)
                .HasMaxLength(50)
                .HasColumnName("Exam_ID");
            entity.Property(e => e.StuCode)
                .HasMaxLength(50)
                .HasColumnName("Stu_Code");
        });

        modelBuilder.Entity<AttendanceTbl>(entity =>
        {
            entity.ToTable("Attendance_tbl");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .HasColumnName("ID");
            entity.Property(e => e.CourseId)
                .HasMaxLength(10)
                .HasColumnName("CourseID");
            entity.Property(e => e.NoOfAttendances).HasColumnName("No_of_Attendances");
            entity.Property(e => e.StudentId)
                .HasMaxLength(10)
                .HasColumnName("StudentID");

            entity.HasOne(d => d.Course).WithMany(p => p.AttendanceTbls)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attendance_tbl_Course_tbl");

            entity.HasOne(d => d.Student).WithMany(p => p.AttendanceTbls)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attendance_tbl_Student_tbl");
        });

        modelBuilder.Entity<CourseTbl>(entity =>
        {
            entity.ToTable("Course_tbl");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .HasColumnName("ID");
            entity.Property(e => e.EdLevel)
                .HasMaxLength(50)
                .HasColumnName("Ed_Level");
            entity.Property(e => e.Name).HasMaxLength(25);
            entity.Property(e => e.Pdfs).HasColumnName("PDFs");
            entity.Property(e => e.Teacher).HasMaxLength(10);

            entity.HasOne(d => d.TeacherNavigation).WithMany(p => p.CourseTbls)
                .HasForeignKey(d => d.Teacher)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Course_tbl_Lecturer_tbl");
        });

        modelBuilder.Entity<EventTbl>(entity =>
        {
            entity.ToTable("Event_tbl");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .HasColumnName("ID");
            entity.Property(e => e.CourseId)
                .HasMaxLength(10)
                .HasColumnName("CourseID");
            entity.Property(e => e.EdLevel)
                .HasMaxLength(50)
                .HasColumnName("Ed_Level");
            entity.Property(e => e.ReleaseDate).HasColumnType("date");
            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(25);

            entity.HasOne(d => d.Course).WithMany(p => p.EventTbls)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK_Event_tbl_Course_tbl");
        });

        modelBuilder.Entity<ExamTbl>(entity =>
        {
            entity.HasKey(e => e.ExamId);

            entity.ToTable("Exam_tbl");

            entity.Property(e => e.ExamId)
                .HasMaxLength(50)
                .HasColumnName("Exam_ID");
            entity.Property(e => e.AvailabilityTime).HasColumnType("datetime");
            entity.Property(e => e.CourseId)
                .HasMaxLength(10)
                .HasColumnName("Course_ID");
            entity.Property(e => e.EdLevel).HasMaxLength(50);
            entity.Property(e => e.Q01).HasMaxLength(50);
            entity.Property(e => e.Q02).HasMaxLength(50);
            entity.Property(e => e.Q03).HasMaxLength(50);
            entity.Property(e => e.Q04).HasMaxLength(50);
            entity.Property(e => e.Q05).HasMaxLength(50);
            entity.Property(e => e.Q06).HasMaxLength(50);
            entity.Property(e => e.Q07).HasMaxLength(50);
            entity.Property(e => e.Q08).HasMaxLength(50);
            entity.Property(e => e.Q09).HasMaxLength(50);
            entity.Property(e => e.Q10).HasMaxLength(50);
            entity.Property(e => e.ReleaseTime).HasColumnType("datetime");
            entity.Property(e => e.Teacher).HasMaxLength(10);
            entity.Property(e => e.TotalMark).HasColumnName("Total_Mark");
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.Course).WithMany(p => p.ExamTbls)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Exam_tbl_Course_tbl");

            entity.HasOne(d => d.Q01Navigation).WithMany(p => p.ExamTblQ01Navigations)
                .HasForeignKey(d => d.Q01)
                .HasConstraintName("FK_Exam_tbl_Question_tbl");

            entity.HasOne(d => d.Q02Navigation).WithMany(p => p.ExamTblQ02Navigations)
                .HasForeignKey(d => d.Q02)
                .HasConstraintName("FK_Exam_tbl_Question_tbl1");

            entity.HasOne(d => d.Q03Navigation).WithMany(p => p.ExamTblQ03Navigations)
                .HasForeignKey(d => d.Q03)
                .HasConstraintName("FK_Exam_tbl_Question_tbl2");

            entity.HasOne(d => d.Q04Navigation).WithMany(p => p.ExamTblQ04Navigations)
                .HasForeignKey(d => d.Q04)
                .HasConstraintName("FK_Exam_tbl_Question_tbl3");

            entity.HasOne(d => d.Q05Navigation).WithMany(p => p.ExamTblQ05Navigations)
                .HasForeignKey(d => d.Q05)
                .HasConstraintName("FK_Exam_tbl_Question_tbl4");

            entity.HasOne(d => d.Q06Navigation).WithMany(p => p.ExamTblQ06Navigations)
                .HasForeignKey(d => d.Q06)
                .HasConstraintName("FK_Exam_tbl_Question_tbl5");

            entity.HasOne(d => d.Q07Navigation).WithMany(p => p.ExamTblQ07Navigations)
                .HasForeignKey(d => d.Q07)
                .HasConstraintName("FK_Exam_tbl_Question_tbl6");

            entity.HasOne(d => d.Q08Navigation).WithMany(p => p.ExamTblQ08Navigations)
                .HasForeignKey(d => d.Q08)
                .HasConstraintName("FK_Exam_tbl_Question_tbl7");

            entity.HasOne(d => d.Q09Navigation).WithMany(p => p.ExamTblQ09Navigations)
                .HasForeignKey(d => d.Q09)
                .HasConstraintName("FK_Exam_tbl_Question_tbl8");

            entity.HasOne(d => d.Q10Navigation).WithMany(p => p.ExamTblQ10Navigations)
                .HasForeignKey(d => d.Q10)
                .HasConstraintName("FK_Exam_tbl_Question_tbl9");

            entity.HasOne(d => d.TeacherNavigation).WithMany(p => p.ExamTbls)
                .HasForeignKey(d => d.Teacher)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Exam_tbl_Lecturer_tbl");
        });

        modelBuilder.Entity<FileTbl>(entity =>
        {
            entity.ToTable("File_tbl");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AssignmentId)
                .HasMaxLength(10)
                .HasColumnName("AssignmentID");
            entity.Property(e => e.CourseId)
                .HasMaxLength(10)
                .HasColumnName("CourseID");
            entity.Property(e => e.FileName).HasMaxLength(50);
            entity.Property(e => e.UploadOn).HasColumnType("datetime");

            entity.HasOne(d => d.Course).WithMany(p => p.FileTbls)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_File_tbl_Course_tbl");
        });

        modelBuilder.Entity<LecturerTbl>(entity =>
        {
            entity.ToTable("Lecturer_tbl");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .HasColumnName("ID");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Image).HasColumnType("image");
            entity.Property(e => e.Name).HasMaxLength(25);
            entity.Property(e => e.NoNotification).HasColumnName("No_Notification");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.RegisteredCourses).HasMaxLength(50);
            entity.Property(e => e.Role).HasMaxLength(25);
            entity.Property(e => e.Salt).HasMaxLength(50);
        });

        modelBuilder.Entity<NotificationTbl>(entity =>
        {
            entity.HasKey(e => e.NotificationId);

            entity.ToTable("Notification_tbl");

            entity.Property(e => e.NotificationId)
                .HasMaxLength(50)
                .HasColumnName("NotificationID");
            entity.Property(e => e.CourseId)
                .HasMaxLength(10)
                .HasColumnName("CourseID");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.EdLevel).HasMaxLength(10);
            entity.Property(e => e.IsNotify).HasColumnName("isNotify");
            entity.Property(e => e.ReleaseDate).HasColumnType("datetime");
            entity.Property(e => e.Role).HasMaxLength(50);

            entity.HasOne(d => d.Course).WithMany(p => p.NotificationTbls)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notification_tbl_Course_tbl");

            entity.HasOne(d => d.EdLevelNavigation).WithMany(p => p.NotificationTbls)
                .HasForeignKey(d => d.EdLevel)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notification_tbl_RegisteredCourses_tbl");
        });

        modelBuilder.Entity<QuestionTbl>(entity =>
        {
            entity.HasKey(e => e.QId).HasName("PK_Question");

            entity.ToTable("Question_tbl");

            entity.Property(e => e.QId)
                .HasMaxLength(50)
                .HasColumnName("Q_ID");
            entity.Property(e => e.Ch01).HasMaxLength(50);
            entity.Property(e => e.Ch02).HasMaxLength(50);
            entity.Property(e => e.Ch03).HasMaxLength(50);
            entity.Property(e => e.Ch04).HasMaxLength(50);
            entity.Property(e => e.CorrectCh)
                .HasMaxLength(50)
                .HasColumnName("Correct_Ch");
            entity.Property(e => e.CourseId)
                .HasMaxLength(10)
                .HasColumnName("CourseID");
            entity.Property(e => e.QuesTitle)
                .HasMaxLength(50)
                .HasColumnName("Ques_Title");
            entity.Property(e => e.QuesType)
                .HasMaxLength(50)
                .HasColumnName("Ques_Type");
            entity.Property(e => e.TotalMark).HasColumnName("Total_Mark");

            entity.HasOne(d => d.Course).WithMany(p => p.QuestionTbls)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK_Question_tbl_Course_tbl");
        });

        modelBuilder.Entity<RegisteredCoursesTbl>(entity =>
        {
            entity.ToTable("RegisteredCourses_tbl");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .HasColumnName("ID");
            entity.Property(e => e.Course01).HasMaxLength(10);
            entity.Property(e => e.Course02).HasMaxLength(10);
            entity.Property(e => e.Course03).HasMaxLength(10);
            entity.Property(e => e.Course04).HasMaxLength(10);
            entity.Property(e => e.Course05).HasMaxLength(10);
            entity.Property(e => e.Course06).HasMaxLength(10);
            entity.Property(e => e.Course07).HasMaxLength(10);

            entity.HasOne(d => d.Course01Navigation).WithMany(p => p.RegisteredCoursesTblCourse01Navigations)
                .HasForeignKey(d => d.Course01)
                .HasConstraintName("FK_RegisteredCourses_tbl_Course_tbl");

            entity.HasOne(d => d.Course02Navigation).WithMany(p => p.RegisteredCoursesTblCourse02Navigations)
                .HasForeignKey(d => d.Course02)
                .HasConstraintName("FK_RegisteredCourses_tbl_Course_tbl1");

            entity.HasOne(d => d.Course03Navigation).WithMany(p => p.RegisteredCoursesTblCourse03Navigations)
                .HasForeignKey(d => d.Course03)
                .HasConstraintName("FK_RegisteredCourses_tbl_Course_tbl2");

            entity.HasOne(d => d.Course04Navigation).WithMany(p => p.RegisteredCoursesTblCourse04Navigations)
                .HasForeignKey(d => d.Course04)
                .HasConstraintName("FK_RegisteredCourses_tbl_Course_tbl3");

            entity.HasOne(d => d.Course05Navigation).WithMany(p => p.RegisteredCoursesTblCourse05Navigations)
                .HasForeignKey(d => d.Course05)
                .HasConstraintName("FK_RegisteredCourses_tbl_Course_tbl4");

            entity.HasOne(d => d.Course06Navigation).WithMany(p => p.RegisteredCoursesTblCourse06Navigations)
                .HasForeignKey(d => d.Course06)
                .HasConstraintName("FK_RegisteredCourses_tbl_Course_tbl5");

            entity.HasOne(d => d.Course07Navigation).WithMany(p => p.RegisteredCoursesTblCourse07Navigations)
                .HasForeignKey(d => d.Course07)
                .HasConstraintName("FK_RegisteredCourses_tbl_Course_tbl6");
        });

        modelBuilder.Entity<ResultTbl>(entity =>
        {
            entity.ToTable("Result_tbl");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .HasColumnName("ID");
            entity.Property(e => e.AchievedMark).HasColumnName("Achieved_Mark");
            entity.Property(e => e.CourseId)
                .HasMaxLength(10)
                .HasColumnName("CourseID");
            entity.Property(e => e.ExamId)
                .HasMaxLength(50)
                .HasColumnName("Exam_ID");
            entity.Property(e => e.StudentId)
                .HasMaxLength(10)
                .HasColumnName("StudentID");
            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.TotalMark).HasColumnName("Total_Mark");

            entity.HasOne(d => d.Course).WithMany(p => p.ResultTbls)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Result_tbl_Course_tbl");

            entity.HasOne(d => d.Exam).WithMany(p => p.ResultTbls)
                .HasForeignKey(d => d.ExamId)
                .HasConstraintName("FK_Result_tbl_Exam_tbl");

            entity.HasOne(d => d.Student).WithMany(p => p.ResultTbls)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Result_tbl_Student_tbl");
        });

        modelBuilder.Entity<StudentTbl>(entity =>
        {
            entity.ToTable("Student_tbl");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .HasColumnName("ID");
            entity.Property(e => e.AttendCourses)
                .HasMaxLength(10)
                .HasColumnName("Attend_Courses");
            entity.Property(e => e.Class).HasMaxLength(50);
            entity.Property(e => e.EdLevel)
                .HasMaxLength(50)
                .HasColumnName("Ed_Level");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Image).HasColumnType("image");
            entity.Property(e => e.Name).HasMaxLength(25);
            entity.Property(e => e.NoNotifications).HasColumnName("No_Notifications");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.RegisteredCourses)
                .HasMaxLength(10)
                .HasColumnName("Registered_Courses");
            entity.Property(e => e.Results).HasMaxLength(10);
            entity.Property(e => e.Salt).HasMaxLength(50);

            entity.HasOne(d => d.AttendCoursesNavigation).WithMany(p => p.StudentTbls)
                .HasForeignKey(d => d.AttendCourses)
                .HasConstraintName("FK_Student_tbl_Attendance_tbl");

            entity.HasOne(d => d.RegisteredCoursesNavigation).WithMany(p => p.StudentTbls)
                .HasForeignKey(d => d.RegisteredCourses)
                .HasConstraintName("FK_Student_tbl_RegisteredCourses_tbl");

            entity.HasOne(d => d.ResultsNavigation).WithMany(p => p.StudentTbls)
                .HasForeignKey(d => d.Results)
                .HasConstraintName("FK_Student_tbl_Result_tbl");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
