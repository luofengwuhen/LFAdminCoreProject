using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;


namespace LFAdminCoreProject.Models
{
    public partial class LFAdminCoreContext : DbContext
    {
        private readonly IConfiguration configuration;

        public LFAdminCoreContext( )
        { 
        }
        public LFAdminCoreContext(IConfiguration Configuration)
        { 
            configuration = Configuration;
        }
        public LFAdminCoreContext(DbContextOptions<LFAdminCoreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TAuthority> TAuthority { get; set; }
        public virtual DbSet<TRole> TRole { get; set; }
        public virtual DbSet<TRoleAuthority> TRoleAuthority { get; set; }
        public virtual DbSet<TUser> TUser { get; set; }
        public virtual DbSet<TUserRole> TUserRole { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //                optionsBuilder.UseSqlServer("Data Source=DESKTOP-B1B83D0\\MSSQLSERVER2;Initial Catalog=LFAdminCore;persist security info=True;user id=sa;password=Yujie123;");

                optionsBuilder.UseSqlServer(Startup.LFAdminCoreContextConnectionStr);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TAuthority>(entity =>
            {
                entity.ToTable("T_Authority");

                entity.HasComment("权限表");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("ID");

                entity.Property(e => e.AuthorityCode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("权限编码");

                entity.Property(e => e.AuthorityName)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("权限名称");

                entity.Property(e => e.ParentAuthorityCode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("父权限编码");
            });

            modelBuilder.Entity<TRole>(entity =>
            {
                entity.ToTable("T_Role");

                entity.HasComment("角色表");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("ID");

                entity.Property(e => e.BanTime)
                    .HasColumnType("datetime")
                    .HasComment("停用时间");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasComment("新增时间");

                entity.Property(e => e.IsBan).HasComment("是否停用");

                entity.Property(e => e.Memo)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasComment("描述");

                entity.Property(e => e.ModifyTime)
                    .HasColumnType("datetime")
                    .HasComment("修改时间");

                entity.Property(e => e.RoleCode)
                    .HasColumnName("Role_Code")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("角色编码");

                entity.Property(e => e.RoleName)
                    .HasColumnName("Role_Name")
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("角色名称");
            });

            modelBuilder.Entity<TRoleAuthority>(entity =>
            {
                entity.ToTable("T_Role_Authority");

                entity.HasComment("角色权限表");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("ID");

                entity.Property(e => e.AuthorityId)
                    .HasColumnName("AuthorityID")
                    .HasComment("权限ID");

                entity.Property(e => e.OperateTime)
                    .HasColumnType("datetime")
                    .HasComment("操作时间");

                entity.Property(e => e.RoleId)
                    .HasColumnName("RoleID")
                    .HasComment("角色ID");
            });

            modelBuilder.Entity<TUser>(entity =>
            {
                entity.ToTable("T_User");

                entity.HasComment("用户表");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("ID");

                entity.Property(e => e.Address)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("地址");

                entity.Property(e => e.BanTime)
                    .HasColumnType("datetime")
                    .HasComment("停用时间");

                entity.Property(e => e.ChineseName)
                    .HasColumnName("Chinese_Name")
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("姓名");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasComment("新增时间");

                entity.Property(e => e.Emial)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("邮箱");

                entity.Property(e => e.IsBan).HasComment("是否停用");

                entity.Property(e => e.Memo)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasComment("备注");

                entity.Property(e => e.ModifyTime)
                    .HasColumnType("datetime")
                    .HasComment("修改时间");

                entity.Property(e => e.Password)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("密码");

                entity.Property(e => e.Phone)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("手机");

                entity.Property(e => e.UserName)
                    .HasColumnName("User_Name")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("用户名");
            });

            modelBuilder.Entity<TUserRole>(entity =>
            {
                entity.ToTable("T_User_Role");

                entity.HasComment("用户角色表");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("ID");

                entity.Property(e => e.OperateTime)
                    .HasColumnType("datetime")
                    .HasComment("操作时间");

                entity.Property(e => e.RoleId)
                    .HasColumnName("Role_ID")
                    .HasComment("角色ID");

                entity.Property(e => e.UserId)
                    .HasColumnName("User_ID")
                    .HasComment("用户ID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
