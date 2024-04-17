using Microsoft.EntityFrameworkCore;

namespace EMS.Workflow.Data.DBContexts
{
    public class WorkflowContext : DbContext
    {
        public WorkflowContext(DbContextOptions<WorkflowContext> options) : base(options)
        {
        }

        #region Tables
        public DbSet<LogActivity.LogActivity> LogActivity { get; set; }
        public DbSet<LogActivity.LogActivityPreloaded> LogActivityPreloaded { get; set; }
        public DbSet<LogActivity.ApplicantLogActivity> ApplicantLogActivity { get; set; }
        public DbSet<LogActivity.ApplicantLogActivityStatusHistory> ApplicantLogActivityStatusHistory { get; set; }
        public DbSet<LogActivity.ApplicantLogActivityComments> ApplicantLogActivityComments { get; set; }
        public DbSet<LogActivity.ApplicantLogActivityAttachment> ApplicantLogActivityAttachment { get; set; }
        public DbSet<LogActivity.EmployeeLogActivity> EmployeeLogActivity { get; set; }
        public DbSet<LogActivity.EmployeeLogActivityStatusHistory> EmployeeLogActivityStatusHistory { get; set; }
        public DbSet<LogActivity.EmployeeLogActivityComments> EmployeeLogActivityComments { get; set; }
        public DbSet<LogActivity.EmployeeLogActivityAttachment> EmployeeLogActivityAttachment { get; set; }
        public DbSet<EmailServerCredential.EmailServerCredential> EmailServerCredential { get; set; }
        public DbSet<EmailServerCredential.EmailLogs> EmailLogs { get; set; }
        public DbSet<EmailServerCredential.CronLogs> CronLogs { get; set; }

        public DbSet<Accountability.Accountability> Accountability { get; set; }
        public DbSet<Accountability.AccountabilityDetails> AccountabilityDetails { get; set; }
        public DbSet<Accountability.EmployeeAccountability> EmployeeAccountability { get; set; }
        public DbSet<Accountability.EmployeeAccountabilityStatusHistory> EmployeeAccountabilityStatusHistory { get; set; }
        public DbSet<Accountability.EmployeeAccountabilityComments> EmployeeAccountabilityComments { get; set; }
        public DbSet<Accountability.EmployeeAccountabilityAttachment> EmployeeAccountabilityAttachment { get; set; }

        public DbSet<Reference.Reference> Reference { get; set; }
        public DbSet<Reference.ReferenceValue> ReferenceValue { get; set; }
        public DbSet<Workflow.Workflow> Workflow { get; set; }
        public DbSet<Workflow.WorkflowStep> WorkflowStep { get; set; }
        public DbSet<Workflow.WorkflowStepApprover> WorkflowStepApprover { get; set; }


        public DbSet<EmployeeScore.EmployeeScore> EmployeeScore { get; set; }
        public DbSet<EmployeeScore.EmployeeScoreApprovalHistory> EmployeeScoreApprovalHistory { get; set; }

        public DbSet<Training.TrainingTemplate> TrainingTemplate { get; set; }
        public DbSet<Training.TrainingTemplateDetails> TrainingTemplateDetails { get; set; }
        public DbSet<Training.EmployeeTraining> EmployeeTraining { get; set; }
        public DbSet<Training.EmployeeTrainingStatusHistory> EmployeeTrainingStatusHistory { get; set; }

        public DbSet<Question.QuestionTable> QuestionTable { get; set; }
        public DbSet<Question.QuestionAnswer> QuestionAnswer { get; set; }
        public DbSet<Question.QuestionEmployeeAnswer> QuestionEmployeeAnswer { get; set; }
        public DbSet<Question.SPGetQuestionEmployeeAnswerExport> SPGetQuestionEmployeeAnswerExport { get; set; }

        public DbSet<Case.Case> CaseMinorAudit { get; set; }
        public DbSet<Case.CaseAttachment> CaseMinorAuditAttachment { get; set; }
        public DbSet<Case.CaseNTE> CaseMinorAuditNTE { get; set; }
        public DbSet<Case.CaseNoa> CaseMinorAuditNoa { get; set; }
        public DbSet<Case.CaseComments> CaseMinorAuditComments { get; set; }
        public DbSet<Case.CaseStatusHistory> CaseMinorAuditStatusHistory { get; set; }

        #endregion Tables

        #region Table Variables
        public DbSet<LogActivity.TableVarApplicantLogActivity> TableVarApplicantLogActivity { get; set; }
        public DbSet<LogActivity.TableVarEmployeeLogActivity> TableVarEmployeeLogActivity { get; set; }
        public DbSet<Workflow.TableVarWorkflow> TableVarWorkflow { get; set; }
        public DbSet<Workflow.TableVarTransaction> TableVarTransaction { get; set; }
        public DbSet<Workflow.TableVarTransactionLastUpdate> TableVarTransactionLastUpdate { get; set; }
        public DbSet<Workflow.TableVarCurrentWorkflowStep> TableVarCurrentWorkflowStep { get; set; }
        public DbSet<LogActivity.TableVarLogActivity> TableVarLogActivity { get; set; }
        public DbSet<LogActivity.TableVarAssignedActivities> TableVarAssignedActivities { get; set; }
        public DbSet<LogActivity.TableVarApplicantLogActivityStatusHistory> TableVarApplicantLogActivityStatusHistory { get; set; }
        public DbSet<LogActivity.TableVarEmployeeLogActivityStatusHistory> TableVarEmployeeLogActivityStatusHistory { get; set; }
        public DbSet<LogActivity.TableVarLogActivityPreLoaded> TableVarLogActivityPreLoaded { get; set; }
        public DbSet<Accountability.TableVarAccountability> TableVarAccountability { get; set; }
        public DbSet<Accountability.TableVarEmployeeAccountability> TableVarEmployeeAccountability { get; set; }
        public DbSet<Accountability.TableVarEmployeeAccountabilityStatusHistory> TableVarEmployeeAccountabilityStatusHistory { get; set; }
        public DbSet<Accountability.TableVarEmployeeAccountabilityStatusPercentage> TableVarEmployeeAccountabilityStatusPercentage { get; set; }
        public DbSet<Accountability.TableVarEmployeeAccountabilityList> TableVarEmployeeAccountabilityList { get; set; }
        public DbSet<Accountability.TableVarAccountabilityDashboard> TableVarAccountabilityDashboard { get; set; }
        public DbSet<Accountability.ClearedEmployee> ClearedEmployee { get; set; }
        public DbSet<Accountability.ClearedEmployeeAttachment> ClearedEmployeeAttachment { get; set; }
        public DbSet<Accountability.ClearedEmployeeComments> ClearedEmployeeComments { get; set; }
        public DbSet<Accountability.ClearedEmployeeStatusHistory> ClearedEmployeeStatusHistory { get; set; }
        public DbSet<Accountability.tvAddClearedEmployee> tvAddClearedEmployee { get; set; }
        public DbSet<Accountability.tvClearedEmployeeList> tvClearedEmployeeList { get; set; }
        public DbSet<Accountability.tvClearedEmployeeByID> tvClearedEmployeeByID { get; set; }
        public DbSet<Accountability.tvClearedEmployeeComments> tvClearedEmployeeComments { get; set; }
        public DbSet<Accountability.tvClearedEmployeeStatusHistory> tvClearedEmployeeStatusHistory { get; set; }
        public DbSet<Accountability.tvEmployeeAccountability> tvEmployeeAccountability { get; set; }

        public DbSet<LogActivity.TableVarChecklist> TableVarChecklist { get; set; }
        public DbSet<LogActivity.TableVarApplicantLogActivityList> TableVarApplicantLogActivityList { get; set; }
        public DbSet<Accountability.TableVarMyAccountabilities> TableVarMyAccountabilities { get; set; }
        public DbSet<Workflow.TableVarWorkflowGetNextWorkflowStep> TableVarWorkflowGetNextWorkflowStep { get; set; }
        public DbSet<Workflow.TableVarWorkflowGetWorkflowByRoleStep> TableVarWorkflowGetWorkflowByRoleStep { get; set; }
        public DbSet<Workflow.TableVarWorkflowGetAllWorkflowStep> TableVarWorkflowGetAllWorkflowStep { get; set; }
        public DbSet<EmployeeScore.TableVarEmployeeScoreStatusHistory> TableVarEmployeeScoreStatusHistory { get; set; }
        public DbSet<Workflow.TableVarWorkflowStepApprover> TableVarWorkflowStepApprover { get; set; }


        public DbSet<Training.TableVarTableTemplate> TableVarTableTemplate { get; set; }
        public DbSet<Training.TableVarEmployeeTraining> TableVarEmployeeTraining { get; set; }
        public DbSet<Training.TableVarEmployeeTrainingScore> TableVarEmployeeTrainingScore { get; set; }
        public DbSet<Training.TableVarEmployeeTrainingStatusHistory> TableVarEmployeeTrainingStatusHistory { get; set; }


        public DbSet<Question.TableVarQuestion> TableVarQuestion { get; set; }
        public DbSet<Question.TableVarQuestionEmployeeAnswer> TableVarQuestionEmployeeAnswer { get; set; }

        #endregion Table Variables
    }
}