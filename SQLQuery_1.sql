-- DROP TABLE IF EXISTS Spring2025_Group3_Profile_Sys;
-- DROP TABLE IF EXISTS Spring2025_Group3_Attendance;

CREATE TABLE Spring2025_Group3_Attendance (
    attendanceID CHAR(16) PRIMARY KEY,  -- Unique identifier for each attendance record
    Email VARCHAR(128) NOT NULL,        -- Foreign key to link to students
    ServiceID CHAR(16) NOT NULL,             -- Foreign key to service
    sectionID INT NOT NULL,             -- Foreign key to section
    ScheduleID INT NOT NULL,            -- Foreign key to schedule
    currentDate DATETIME DEFAULT GETDATE(), -- Timestamp of the attendance record
    attendanceStatus VARCHAR(50) NOT NULL CHECK (attendanceStatus IN ('Present', 'Absent', 'Late')),

    -- Define Foreign Key Constraints
    CONSTRAINT FK_Attendance_Email FOREIGN KEY (Email) 
        REFERENCES Spring2023_Group1_Profile_Sys(Email) ON DELETE CASCADE,

    CONSTRAINT FK_Attendance_Service FOREIGN KEY (ServiceID) 
        REFERENCES Spring2023_Group1_Services(ServiceID) ON DELETE CASCADE,


-- We updated these bottom two databases. The years and groups used were initially wrong
    CONSTRAINT FK_Attendance_Section FOREIGN KEY (sectionID) 
        REFERENCES Spring2024_Group2_Sections(sectionID) ON DELETE CASCADE,

    CONSTRAINT FK_Attendance_Schedule FOREIGN KEY (ScheduleID) 
        REFERENCES Spring2024_Group2_Schedule(ScheduleID) ON DELETE CASCADE
);
