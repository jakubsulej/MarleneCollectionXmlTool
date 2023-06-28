using MarleneCollectionXmlTool.DBAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace MarleneCollectionXmlTool.DBAccessLayer;

public partial class WoocommerceDbContext : DbContext
{
    public WoocommerceDbContext()
    {
    }

    public WoocommerceDbContext(DbContextOptions<WoocommerceDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<WpActionschedulerAction> WpActionschedulerActions { get; set; }

    public virtual DbSet<WpActionschedulerClaim> WpActionschedulerClaims { get; set; }

    public virtual DbSet<WpActionschedulerGroup> WpActionschedulerGroups { get; set; }

    public virtual DbSet<WpActionschedulerLog> WpActionschedulerLogs { get; set; }

    public virtual DbSet<WpBerocketTermmetum> WpBerocketTermmeta { get; set; }

    public virtual DbSet<WpBlcFilter> WpBlcFilters { get; set; }

    public virtual DbSet<WpBlcInstance> WpBlcInstances { get; set; }

    public virtual DbSet<WpBlcLink> WpBlcLinks { get; set; }

    public virtual DbSet<WpBlcSynch> WpBlcSynches { get; set; }

    public virtual DbSet<WpComment> WpComments { get; set; }

    public virtual DbSet<WpCommentmetum> WpCommentmeta { get; set; }

    public virtual DbSet<WpGdprCcOption> WpGdprCcOptions { get; set; }

    public virtual DbSet<WpGlaAttributeMappingRule> WpGlaAttributeMappingRules { get; set; }

    public virtual DbSet<WpGlaBudgetRecommendation> WpGlaBudgetRecommendations { get; set; }

    public virtual DbSet<WpGlaMerchantIssue> WpGlaMerchantIssues { get; set; }

    public virtual DbSet<WpGlaShippingRate> WpGlaShippingRates { get; set; }

    public virtual DbSet<WpGlaShippingTime> WpGlaShippingTimes { get; set; }

    public virtual DbSet<WpImageHoverUltimateList> WpImageHoverUltimateLists { get; set; }

    public virtual DbSet<WpImageHoverUltimateStyle> WpImageHoverUltimateStyles { get; set; }

    public virtual DbSet<WpLink> WpLinks { get; set; }

    public virtual DbSet<WpOption> WpOptions { get; set; }

    public virtual DbSet<WpOxiDivImport> WpOxiDivImports { get; set; }

    public virtual DbSet<WpPmxiFile> WpPmxiFiles { get; set; }

    public virtual DbSet<WpPmxiHash> WpPmxiHashes { get; set; }

    public virtual DbSet<WpPmxiHistory> WpPmxiHistories { get; set; }

    public virtual DbSet<WpPmxiImage> WpPmxiImages { get; set; }

    public virtual DbSet<WpPmxiImport> WpPmxiImports { get; set; }

    public virtual DbSet<WpPmxiPost> WpPmxiPosts { get; set; }

    public virtual DbSet<WpPmxiTemplate> WpPmxiTemplates { get; set; }

    public virtual DbSet<WpPost> WpPosts { get; set; }

    public virtual DbSet<WpPostmetum> WpPostmeta { get; set; }

    public virtual DbSet<WpSnippet> WpSnippets { get; set; }

    public virtual DbSet<WpTerm> WpTerms { get; set; }

    public virtual DbSet<WpTermRelationship> WpTermRelationships { get; set; }

    public virtual DbSet<WpTermTaxonomy> WpTermTaxonomies { get; set; }

    public virtual DbSet<WpTermmetum> WpTermmeta { get; set; }

    public virtual DbSet<WpTmTask> WpTmTasks { get; set; }

    public virtual DbSet<WpTmTaskmetum> WpTmTaskmeta { get; set; }

    public virtual DbSet<WpUser> WpUsers { get; set; }

    public virtual DbSet<WpUsermetum> WpUsermeta { get; set; }

    public virtual DbSet<WpWcAdminNote> WpWcAdminNotes { get; set; }

    public virtual DbSet<WpWcAdminNoteAction> WpWcAdminNoteActions { get; set; }

    public virtual DbSet<WpWcCategoryLookup> WpWcCategoryLookups { get; set; }

    public virtual DbSet<WpWcCustomerLookup> WpWcCustomerLookups { get; set; }

    public virtual DbSet<WpWcDownloadLog> WpWcDownloadLogs { get; set; }

    public virtual DbSet<WpWcOrderCouponLookup> WpWcOrderCouponLookups { get; set; }

    public virtual DbSet<WpWcOrderProductLookup> WpWcOrderProductLookups { get; set; }

    public virtual DbSet<WpWcOrderStat> WpWcOrderStats { get; set; }

    public virtual DbSet<WpWcOrderTaxLookup> WpWcOrderTaxLookups { get; set; }

    public virtual DbSet<WpWcProductAttributesLookup> WpWcProductAttributesLookups { get; set; }

    public virtual DbSet<WpWcProductDownloadDirectory> WpWcProductDownloadDirectories { get; set; }

    public virtual DbSet<WpWcProductMetaLookup> WpWcProductMetaLookups { get; set; }

    public virtual DbSet<WpWcRateLimit> WpWcRateLimits { get; set; }

    public virtual DbSet<WpWcReservedStock> WpWcReservedStocks { get; set; }

    public virtual DbSet<WpWcTaxRateClass> WpWcTaxRateClasses { get; set; }

    public virtual DbSet<WpWcWebhook> WpWcWebhooks { get; set; }

    public virtual DbSet<WpWoocommerceApiKey> WpWoocommerceApiKeys { get; set; }

    public virtual DbSet<WpWoocommerceAttributeTaxonomy> WpWoocommerceAttributeTaxonomies { get; set; }

    public virtual DbSet<WpWoocommerceDownloadableProductPermission> WpWoocommerceDownloadableProductPermissions { get; set; }

    public virtual DbSet<WpWoocommerceLog> WpWoocommerceLogs { get; set; }

    public virtual DbSet<WpWoocommerceOrderItem> WpWoocommerceOrderItems { get; set; }

    public virtual DbSet<WpWoocommerceOrderItemmetum> WpWoocommerceOrderItemmeta { get; set; }

    public virtual DbSet<WpWoocommercePaymentToken> WpWoocommercePaymentTokens { get; set; }

    public virtual DbSet<WpWoocommercePaymentTokenmetum> WpWoocommercePaymentTokenmeta { get; set; }

    public virtual DbSet<WpWoocommerceSession> WpWoocommerceSessions { get; set; }

    public virtual DbSet<WpWoocommerceShippingZone> WpWoocommerceShippingZones { get; set; }

    public virtual DbSet<WpWoocommerceShippingZoneLocation> WpWoocommerceShippingZoneLocations { get; set; }

    public virtual DbSet<WpWoocommerceShippingZoneMethod> WpWoocommerceShippingZoneMethods { get; set; }

    public virtual DbSet<WpWoocommerceTaxRate> WpWoocommerceTaxRates { get; set; }

    public virtual DbSet<WpWoocommerceTaxRateLocation> WpWoocommerceTaxRateLocations { get; set; }

    public virtual DbSet<WpWssLog> WpWssLogs { get; set; }

    public virtual DbSet<WpXsgSitemapMetum> WpXsgSitemapMeta { get; set; }

    public virtual DbSet<WpYithWcwl> WpYithWcwls { get; set; }

    public virtual DbSet<WpYithWcwlList> WpYithWcwlLists { get; set; }

    public virtual DbSet<WpYoastIndexable> WpYoastIndexables { get; set; }

    public virtual DbSet<WpYoastIndexableHierarchy> WpYoastIndexableHierarchies { get; set; }

    public virtual DbSet<WpYoastMigration> WpYoastMigrations { get; set; }

    public virtual DbSet<WpYoastPrimaryTerm> WpYoastPrimaryTerms { get; set; }

    public virtual DbSet<WpYoastSeoLink> WpYoastSeoLinks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WpActionschedulerAction>(entity =>
        {
            entity.HasKey(e => e.ActionId).HasName("PRIMARY");

            entity.ToTable("wp_actionscheduler_actions");

            entity.HasIndex(e => e.Args, "args");

            entity.HasIndex(e => new { e.ClaimId, e.Status, e.ScheduledDateGmt }, "claim_id_status_scheduled_date_gmt");

            entity.HasIndex(e => e.GroupId, "group_id");

            entity.HasIndex(e => e.Hook, "hook");

            entity.HasIndex(e => e.LastAttemptGmt, "last_attempt_gmt");

            entity.HasIndex(e => e.ScheduledDateGmt, "scheduled_date_gmt");

            entity.HasIndex(e => e.Status, "status");

            entity.Property(e => e.ActionId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("action_id");
            entity.Property(e => e.Args)
                .HasMaxLength(191)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("args");
            entity.Property(e => e.Attempts)
                .HasColumnType("int(11)")
                .HasColumnName("attempts");
            entity.Property(e => e.ClaimId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("claim_id");
            entity.Property(e => e.ExtendedArgs)
                .HasMaxLength(8000)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("extended_args");
            entity.Property(e => e.GroupId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("group_id");
            entity.Property(e => e.Hook)
                .IsRequired()
                .HasMaxLength(191)
                .HasColumnName("hook");
            entity.Property(e => e.LastAttemptGmt)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("last_attempt_gmt");
            entity.Property(e => e.LastAttemptLocal)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("last_attempt_local");
            entity.Property(e => e.Schedule)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("schedule");
            entity.Property(e => e.ScheduledDateGmt)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("scheduled_date_gmt");
            entity.Property(e => e.ScheduledDateLocal)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("scheduled_date_local");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("status");
        });

        modelBuilder.Entity<WpActionschedulerClaim>(entity =>
        {
            entity.HasKey(e => e.ClaimId).HasName("PRIMARY");

            entity.ToTable("wp_actionscheduler_claims");

            entity.HasIndex(e => e.DateCreatedGmt, "date_created_gmt");

            entity.Property(e => e.ClaimId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("claim_id");
            entity.Property(e => e.DateCreatedGmt)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("date_created_gmt");
        });

        modelBuilder.Entity<WpActionschedulerGroup>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("PRIMARY");

            entity.ToTable("wp_actionscheduler_groups");

            entity.HasIndex(e => e.Slug, "slug");

            entity.Property(e => e.GroupId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("group_id");
            entity.Property(e => e.Slug)
                .IsRequired()
                .HasColumnName("slug");
        });

        modelBuilder.Entity<WpActionschedulerLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PRIMARY");

            entity.ToTable("wp_actionscheduler_logs");

            entity.HasIndex(e => e.ActionId, "action_id");

            entity.HasIndex(e => e.LogDateGmt, "log_date_gmt");

            entity.Property(e => e.LogId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("log_id");
            entity.Property(e => e.ActionId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("action_id");
            entity.Property(e => e.LogDateGmt)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("log_date_gmt");
            entity.Property(e => e.LogDateLocal)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("log_date_local");
            entity.Property(e => e.Message)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("message");
        });

        modelBuilder.Entity<WpBerocketTermmetum>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("wp_berocket_termmeta");

            entity.HasIndex(e => e.MetaId, "meta_id").IsUnique();

            entity.Property(e => e.BerocketTermId)
                .HasColumnType("bigint(20)")
                .HasColumnName("berocket_term_id");
            entity.Property(e => e.MetaId)
                .ValueGeneratedOnAdd()
                .HasColumnType("bigint(20)")
                .HasColumnName("meta_id");
            entity.Property(e => e.MetaKey)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("meta_key");
            entity.Property(e => e.MetaValue)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("meta_value");
        });

        modelBuilder.Entity<WpBlcFilter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("wp_blc_filters");

            entity.Property(e => e.Id)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Params)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("params");
        });

        modelBuilder.Entity<WpBlcInstance>(entity =>
        {
            entity.HasKey(e => e.InstanceId).HasName("PRIMARY");

            entity.ToTable("wp_blc_instances");

            entity.HasIndex(e => e.LinkId, "link_id");

            entity.HasIndex(e => e.ParserType, "parser_type");

            entity.HasIndex(e => new { e.ContainerType, e.ContainerId }, "source_id");

            entity.Property(e => e.InstanceId)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("instance_id");
            entity.Property(e => e.ContainerField)
                .IsRequired()
                .HasMaxLength(250)
                .HasDefaultValueSql("''''''")
                .HasColumnName("container_field");
            entity.Property(e => e.ContainerId)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("container_id");
            entity.Property(e => e.ContainerType)
                .IsRequired()
                .HasMaxLength(40)
                .HasDefaultValueSql("'''post'''")
                .HasColumnName("container_type");
            entity.Property(e => e.LinkContext)
                .IsRequired()
                .HasMaxLength(250)
                .HasDefaultValueSql("''''''")
                .HasColumnName("link_context");
            entity.Property(e => e.LinkId)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("link_id");
            entity.Property(e => e.LinkText)
                .IsRequired()
                .HasDefaultValueSql("''''''")
                .HasColumnType("text")
                .HasColumnName("link_text");
            entity.Property(e => e.ParserType)
                .IsRequired()
                .HasMaxLength(40)
                .HasDefaultValueSql("'''link'''")
                .HasColumnName("parser_type");
            entity.Property(e => e.RawUrl)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("raw_url");
        });

        modelBuilder.Entity<WpBlcLink>(entity =>
        {
            entity.HasKey(e => e.LinkId).HasName("PRIMARY");

            entity.ToTable("wp_blc_links");

            entity.HasIndex(e => e.Broken, "broken");

            entity.HasIndex(e => e.FinalUrl, "final_url");

            entity.HasIndex(e => e.HttpCode, "http_code");

            entity.HasIndex(e => e.Url, "url");

            entity.Property(e => e.LinkId)
                .HasColumnType("int(20) unsigned")
                .HasColumnName("link_id");
            entity.Property(e => e.BeingChecked).HasColumnName("being_checked");
            entity.Property(e => e.Broken)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("broken");
            entity.Property(e => e.CheckCount)
                .HasColumnType("int(4) unsigned")
                .HasColumnName("check_count");
            entity.Property(e => e.Dismissed).HasColumnName("dismissed");
            entity.Property(e => e.FalsePositive).HasColumnName("false_positive");
            entity.Property(e => e.FinalUrl)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("final_url");
            entity.Property(e => e.FirstFailure)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("first_failure");
            entity.Property(e => e.HttpCode)
                .HasColumnType("smallint(6)")
                .HasColumnName("http_code");
            entity.Property(e => e.LastCheck)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("last_check");
            entity.Property(e => e.LastCheckAttempt)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("last_check_attempt");
            entity.Property(e => e.LastSuccess)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("last_success");
            entity.Property(e => e.Log)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("log");
            entity.Property(e => e.MayRecheck)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("may_recheck");
            entity.Property(e => e.RedirectCount)
                .HasColumnType("smallint(5) unsigned")
                .HasColumnName("redirect_count");
            entity.Property(e => e.RequestDuration).HasColumnName("request_duration");
            entity.Property(e => e.ResultHash)
                .IsRequired()
                .HasMaxLength(200)
                .HasDefaultValueSql("''''''")
                .HasColumnName("result_hash");
            entity.Property(e => e.StatusCode)
                .HasMaxLength(100)
                .HasDefaultValueSql("''''''")
                .HasColumnName("status_code");
            entity.Property(e => e.StatusText)
                .HasMaxLength(250)
                .HasDefaultValueSql("''''''")
                .HasColumnName("status_text");
            entity.Property(e => e.Timeout)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("timeout");
            entity.Property(e => e.Url)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("url");
            entity.Property(e => e.Warning)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("warning");
        });

        modelBuilder.Entity<WpBlcSynch>(entity =>
        {
            entity.HasKey(e => new { e.ContainerType, e.ContainerId }).HasName("PRIMARY");

            entity.ToTable("wp_blc_synch");

            entity.HasIndex(e => e.Synched, "synched");

            entity.Property(e => e.ContainerType)
                .HasMaxLength(40)
                .HasColumnName("container_type");
            entity.Property(e => e.ContainerId)
                .HasColumnType("int(20) unsigned")
                .HasColumnName("container_id");
            entity.Property(e => e.LastSynch)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("last_synch");
            entity.Property(e => e.Synched)
                .HasColumnType("tinyint(2) unsigned")
                .HasColumnName("synched");
        });

        modelBuilder.Entity<WpComment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PRIMARY");

            entity.ToTable("wp_comments");

            entity.HasIndex(e => new { e.CommentApproved, e.CommentDateGmt }, "comment_approved_date_gmt");

            entity.HasIndex(e => e.CommentAuthorEmail, "comment_author_email");

            entity.HasIndex(e => e.CommentDateGmt, "comment_date_gmt");

            entity.HasIndex(e => e.CommentParent, "comment_parent");

            entity.HasIndex(e => e.CommentPostId, "comment_post_ID");

            entity.HasIndex(e => e.CommentType, "woo_idx_comment_type");

            entity.Property(e => e.CommentId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("comment_ID");
            entity.Property(e => e.CommentAgent)
                .IsRequired()
                .HasMaxLength(255)
                .HasDefaultValueSql("''''''")
                .HasColumnName("comment_agent");
            entity.Property(e => e.CommentApproved)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValueSql("'''1'''")
                .HasColumnName("comment_approved");
            entity.Property(e => e.CommentAuthor)
                .IsRequired()
                .HasColumnType("tinytext")
                .HasColumnName("comment_author");
            entity.Property(e => e.CommentAuthorEmail)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValueSql("''''''")
                .HasColumnName("comment_author_email");
            entity.Property(e => e.CommentAuthorIp)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValueSql("''''''")
                .HasColumnName("comment_author_IP");
            entity.Property(e => e.CommentAuthorUrl)
                .IsRequired()
                .HasMaxLength(200)
                .HasDefaultValueSql("''''''")
                .HasColumnName("comment_author_url");
            entity.Property(e => e.CommentContent)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("comment_content");
            entity.Property(e => e.CommentDate)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("comment_date");
            entity.Property(e => e.CommentDateGmt)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("comment_date_gmt");
            entity.Property(e => e.CommentKarma)
                .HasColumnType("int(11)")
                .HasColumnName("comment_karma");
            entity.Property(e => e.CommentParent)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("comment_parent");
            entity.Property(e => e.CommentPostId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("comment_post_ID");
            entity.Property(e => e.CommentType)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValueSql("'''comment'''")
                .HasColumnName("comment_type");
            entity.Property(e => e.UserId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("user_id");
        });

        modelBuilder.Entity<WpCommentmetum>(entity =>
        {
            entity.HasKey(e => e.MetaId).HasName("PRIMARY");

            entity.ToTable("wp_commentmeta");

            entity.HasIndex(e => e.CommentId, "comment_id");

            entity.HasIndex(e => e.MetaKey, "meta_key");

            entity.Property(e => e.MetaId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("meta_id");
            entity.Property(e => e.CommentId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("comment_id");
            entity.Property(e => e.MetaKey)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("meta_key");
            entity.Property(e => e.MetaValue)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("meta_value");
        });

        modelBuilder.Entity<WpGdprCcOption>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("wp_gdpr_cc_options");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Extras)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("extras");
            entity.Property(e => e.OptionKey)
                .IsRequired()
                .HasMaxLength(255)
                .HasDefaultValueSql("'''1'''")
                .HasColumnName("option_key");
            entity.Property(e => e.OptionValue)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("option_value");
            entity.Property(e => e.SiteId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11)")
                .HasColumnName("site_id");
        });

        modelBuilder.Entity<WpGlaAttributeMappingRule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("wp_gla_attribute_mapping_rules");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20)")
                .HasColumnName("id");
            entity.Property(e => e.Attribute)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("attribute");
            entity.Property(e => e.Categories)
                .HasDefaultValueSql("''''''")
                .HasColumnType("text")
                .HasColumnName("categories");
            entity.Property(e => e.CategoryConditionType)
                .IsRequired()
                .HasMaxLength(10)
                .HasColumnName("category_condition_type");
            entity.Property(e => e.Source)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("source");
        });

        modelBuilder.Entity<WpGlaBudgetRecommendation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("wp_gla_budget_recommendations");

            entity.HasIndex(e => new { e.Country, e.Currency }, "country_currency").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20)")
                .HasColumnName("id");
            entity.Property(e => e.Country)
                .IsRequired()
                .HasMaxLength(2)
                .HasColumnName("country");
            entity.Property(e => e.Currency)
                .IsRequired()
                .HasMaxLength(3)
                .HasColumnName("currency");
            entity.Property(e => e.DailyBudgetHigh)
                .HasColumnType("int(11)")
                .HasColumnName("daily_budget_high");
            entity.Property(e => e.DailyBudgetLow)
                .HasColumnType("int(11)")
                .HasColumnName("daily_budget_low");
        });

        modelBuilder.Entity<WpGlaMerchantIssue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("wp_gla_merchant_issues");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20)")
                .HasColumnName("id");
            entity.Property(e => e.Action)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("action");
            entity.Property(e => e.ActionUrl)
                .IsRequired()
                .HasMaxLength(1024)
                .HasColumnName("action_url");
            entity.Property(e => e.ApplicableCountries)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("applicable_countries");
            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Issue)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("issue");
            entity.Property(e => e.Product)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("product");
            entity.Property(e => e.ProductId)
                .HasColumnType("bigint(20)")
                .HasColumnName("product_id");
            entity.Property(e => e.Severity)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValueSql("'''warning'''")
                .HasColumnName("severity");
            entity.Property(e => e.Source)
                .IsRequired()
                .HasMaxLength(10)
                .HasDefaultValueSql("'''mc'''")
                .HasColumnName("source");
            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(10)
                .HasDefaultValueSql("'''product'''")
                .HasColumnName("type");
        });

        modelBuilder.Entity<WpGlaShippingRate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("wp_gla_shipping_rates");

            entity.HasIndex(e => e.Country, "country");

            entity.HasIndex(e => e.Currency, "currency");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20)")
                .HasColumnName("id");
            entity.Property(e => e.Country)
                .IsRequired()
                .HasMaxLength(2)
                .HasColumnName("country");
            entity.Property(e => e.Currency)
                .IsRequired()
                .HasMaxLength(3)
                .HasColumnName("currency");
            entity.Property(e => e.Options)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("options");
            entity.Property(e => e.Rate).HasColumnName("rate");
        });

        modelBuilder.Entity<WpGlaShippingTime>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("wp_gla_shipping_times");

            entity.HasIndex(e => e.Country, "country");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20)")
                .HasColumnName("id");
            entity.Property(e => e.Country)
                .IsRequired()
                .HasMaxLength(2)
                .HasColumnName("country");
            entity.Property(e => e.Time)
                .HasColumnType("bigint(20)")
                .HasColumnName("time");
        });

        modelBuilder.Entity<WpImageHoverUltimateList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("wp_image_hover_ultimate_list");

            entity.Property(e => e.Id)
                .HasColumnType("mediumint(5)")
                .HasColumnName("id");
            entity.Property(e => e.Rawdata)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("rawdata");
            entity.Property(e => e.Styleid)
                .HasColumnType("mediumint(6)")
                .HasColumnName("styleid");
        });

        modelBuilder.Entity<WpImageHoverUltimateStyle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("wp_image_hover_ultimate_style");

            entity.Property(e => e.Id)
                .HasColumnType("mediumint(5)")
                .HasColumnName("id");
            entity.Property(e => e.FontFamily)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("font_family");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Rawdata)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("rawdata");
            entity.Property(e => e.StyleName)
                .IsRequired()
                .HasMaxLength(40)
                .HasColumnName("style_name");
            entity.Property(e => e.Stylesheet)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("stylesheet");
        });

        modelBuilder.Entity<WpLink>(entity =>
        {
            entity.HasKey(e => e.LinkId).HasName("PRIMARY");

            entity.ToTable("wp_links");

            entity.HasIndex(e => e.LinkVisible, "link_visible");

            entity.Property(e => e.LinkId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("link_id");
            entity.Property(e => e.LinkDescription)
                .IsRequired()
                .HasMaxLength(255)
                .HasDefaultValueSql("''''''")
                .HasColumnName("link_description");
            entity.Property(e => e.LinkImage)
                .IsRequired()
                .HasMaxLength(255)
                .HasDefaultValueSql("''''''")
                .HasColumnName("link_image");
            entity.Property(e => e.LinkName)
                .IsRequired()
                .HasMaxLength(255)
                .HasDefaultValueSql("''''''")
                .HasColumnName("link_name");
            entity.Property(e => e.LinkNotes)
                .IsRequired()
                .HasColumnType("mediumtext")
                .HasColumnName("link_notes");
            entity.Property(e => e.LinkOwner)
                .HasDefaultValueSql("'1'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("link_owner");
            entity.Property(e => e.LinkRating)
                .HasColumnType("int(11)")
                .HasColumnName("link_rating");
            entity.Property(e => e.LinkRel)
                .IsRequired()
                .HasMaxLength(255)
                .HasDefaultValueSql("''''''")
                .HasColumnName("link_rel");
            entity.Property(e => e.LinkRss)
                .IsRequired()
                .HasMaxLength(255)
                .HasDefaultValueSql("''''''")
                .HasColumnName("link_rss");
            entity.Property(e => e.LinkTarget)
                .IsRequired()
                .HasMaxLength(25)
                .HasDefaultValueSql("''''''")
                .HasColumnName("link_target");
            entity.Property(e => e.LinkUpdated)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("link_updated");
            entity.Property(e => e.LinkUrl)
                .IsRequired()
                .HasMaxLength(255)
                .HasDefaultValueSql("''''''")
                .HasColumnName("link_url");
            entity.Property(e => e.LinkVisible)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValueSql("'''Y'''")
                .HasColumnName("link_visible");
        });

        modelBuilder.Entity<WpOption>(entity =>
        {
            entity.HasKey(e => e.OptionId).HasName("PRIMARY");

            entity.ToTable("wp_options");

            entity.HasIndex(e => e.Autoload, "autoload");

            entity.HasIndex(e => e.OptionName, "option_name").IsUnique();

            entity.Property(e => e.OptionId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("option_id");
            entity.Property(e => e.Autoload)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValueSql("'''yes'''")
                .HasColumnName("autoload");
            entity.Property(e => e.OptionName)
                .IsRequired()
                .HasMaxLength(191)
                .HasDefaultValueSql("''''''")
                .HasColumnName("option_name");
            entity.Property(e => e.OptionValue)
                .IsRequired()
                .HasColumnName("option_value");
        });

        modelBuilder.Entity<WpOxiDivImport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("wp_oxi_div_import");

            entity.Property(e => e.Id)
                .HasColumnType("mediumint(5)")
                .HasColumnName("id");
            entity.Property(e => e.Font)
                .HasMaxLength(200)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("font");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("name");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("type");
        });

        modelBuilder.Entity<WpPmxiFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("wp_pmxi_files");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.ImportId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("import_id");
            entity.Property(e => e.Name)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("name");
            entity.Property(e => e.Path)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("path");
            entity.Property(e => e.RegisteredOn)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("registered_on");
        });

        modelBuilder.Entity<WpPmxiHash>(entity =>
        {
            entity.HasKey(e => e.Hash).HasName("PRIMARY");

            entity.ToTable("wp_pmxi_hash");

            entity.Property(e => e.Hash)
                .HasMaxLength(16)
                .IsFixedLength()
                .HasColumnName("hash");
            entity.Property(e => e.ImportId)
                .HasColumnType("smallint(5) unsigned")
                .HasColumnName("import_id");
            entity.Property(e => e.PostId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("post_id");
            entity.Property(e => e.PostType)
                .IsRequired()
                .HasMaxLength(32)
                .HasDefaultValueSql("''''''")
                .HasColumnName("post_type");
        });

        modelBuilder.Entity<WpPmxiHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("wp_pmxi_history");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.ImportId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("import_id");
            entity.Property(e => e.Summary)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("summary");
            entity.Property(e => e.TimeRun)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("time_run");
            entity.Property(e => e.Type)
                .IsRequired()
                .HasDefaultValueSql("''''''")
                .HasColumnType("enum('manual','processing','trigger','continue','cli','')")
                .HasColumnName("type");
        });

        modelBuilder.Entity<WpPmxiImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("wp_pmxi_images");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.AttachmentId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("attachment_id");
            entity.Property(e => e.ImageFilename)
                .IsRequired()
                .HasMaxLength(900)
                .HasDefaultValueSql("''''''")
                .HasColumnName("image_filename");
            entity.Property(e => e.ImageUrl)
                .IsRequired()
                .HasMaxLength(900)
                .HasDefaultValueSql("''''''")
                .HasColumnName("image_url");
        });

        modelBuilder.Entity<WpPmxiImport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("wp_pmxi_imports");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Canceled).HasColumnName("canceled");
            entity.Property(e => e.CanceledOn)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("canceled_on");
            entity.Property(e => e.Count)
                .HasColumnType("bigint(20)")
                .HasColumnName("count");
            entity.Property(e => e.Created)
                .HasColumnType("bigint(20)")
                .HasColumnName("created");
            entity.Property(e => e.Deleted)
                .HasColumnType("bigint(20)")
                .HasColumnName("deleted");
            entity.Property(e => e.Executing).HasColumnName("executing");
            entity.Property(e => e.Failed).HasColumnName("failed");
            entity.Property(e => e.FailedOn)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("failed_on");
            entity.Property(e => e.FeedType)
                .IsRequired()
                .HasDefaultValueSql("''''''")
                .HasColumnType("enum('xml','csv','zip','gz','')")
                .HasColumnName("feed_type");
            entity.Property(e => e.FirstImport)
                .HasDefaultValueSql("'current_timestamp()'")
                .HasColumnType("timestamp")
                .HasColumnName("first_import");
            entity.Property(e => e.FriendlyName)
                .IsRequired()
                .HasMaxLength(255)
                .HasDefaultValueSql("''''''")
                .HasColumnName("friendly_name");
            entity.Property(e => e.Imported)
                .HasColumnType("bigint(20)")
                .HasColumnName("imported");
            entity.Property(e => e.Iteration)
                .HasColumnType("bigint(20)")
                .HasColumnName("iteration");
            entity.Property(e => e.LastActivity)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("last_activity");
            entity.Property(e => e.Name)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("name");
            entity.Property(e => e.Options)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("options");
            entity.Property(e => e.ParentImportId)
                .HasColumnType("bigint(20)")
                .HasColumnName("parent_import_id");
            entity.Property(e => e.Path)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("path");
            entity.Property(e => e.Processing).HasColumnName("processing");
            entity.Property(e => e.QueueChunkNumber)
                .HasColumnType("bigint(20)")
                .HasColumnName("queue_chunk_number");
            entity.Property(e => e.RegisteredOn)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("registered_on");
            entity.Property(e => e.RootElement)
                .HasMaxLength(255)
                .HasDefaultValueSql("''''''")
                .HasColumnName("root_element");
            entity.Property(e => e.SettingsUpdateOn)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("settings_update_on");
            entity.Property(e => e.Skipped)
                .HasColumnType("bigint(20)")
                .HasColumnName("skipped");
            entity.Property(e => e.Triggered).HasColumnName("triggered");
            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(32)
                .HasDefaultValueSql("''''''")
                .HasColumnName("type");
            entity.Property(e => e.Updated)
                .HasColumnType("bigint(20)")
                .HasColumnName("updated");
            entity.Property(e => e.Xpath)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("xpath");
        });

        modelBuilder.Entity<WpPmxiPost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("wp_pmxi_posts");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.ImportId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("import_id");
            entity.Property(e => e.Iteration)
                .HasColumnType("bigint(20)")
                .HasColumnName("iteration");
            entity.Property(e => e.PostId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("post_id");
            entity.Property(e => e.ProductKey)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("product_key");
            entity.Property(e => e.Specified).HasColumnName("specified");
            entity.Property(e => e.UniqueKey)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("unique_key");
        });

        modelBuilder.Entity<WpPmxiTemplate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("wp_pmxi_templates");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Content)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("content");
            entity.Property(e => e.FixCharacters).HasColumnName("fix_characters");
            entity.Property(e => e.IsKeepLinebreaks).HasColumnName("is_keep_linebreaks");
            entity.Property(e => e.IsLeaveHtml).HasColumnName("is_leave_html");
            entity.Property(e => e.Meta)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("meta");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200)
                .HasDefaultValueSql("''''''")
                .HasColumnName("name");
            entity.Property(e => e.Options)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("options");
            entity.Property(e => e.Scheduled)
                .IsRequired()
                .HasMaxLength(64)
                .HasDefaultValueSql("''''''")
                .HasColumnName("scheduled");
            entity.Property(e => e.Title)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("title");
        });

        modelBuilder.Entity<WpPost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("wp_posts");

            entity.HasIndex(e => e.PostAuthor, "post_author");

            entity.HasIndex(e => e.PostName, "post_name");

            entity.HasIndex(e => e.PostParent, "post_parent");

            entity.HasIndex(e => new { e.PostType, e.PostStatus, e.PostDate, e.Id }, "type_status_date");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ID");
            entity.Property(e => e.CommentCount)
                .HasColumnType("bigint(20)")
                .HasColumnName("comment_count");
            entity.Property(e => e.CommentStatus)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValueSql("'''open'''")
                .HasColumnName("comment_status");
            entity.Property(e => e.Guid)
                .IsRequired()
                .HasMaxLength(255)
                .HasDefaultValueSql("''''''")
                .HasColumnName("guid");
            entity.Property(e => e.MenuOrder)
                .HasColumnType("int(11)")
                .HasColumnName("menu_order");
            entity.Property(e => e.PingStatus)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValueSql("'''open'''")
                .HasColumnName("ping_status");
            entity.Property(e => e.Pinged)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("pinged");
            entity.Property(e => e.PostAuthor)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("post_author");
            entity.Property(e => e.PostContent)
                .IsRequired()
                .HasColumnName("post_content");
            entity.Property(e => e.PostContentFiltered)
                .IsRequired()
                .HasColumnName("post_content_filtered");
            entity.Property(e => e.PostDate)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("post_date");
            entity.Property(e => e.PostDateGmt)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("post_date_gmt");
            entity.Property(e => e.PostExcerpt)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("post_excerpt");
            entity.Property(e => e.PostMimeType)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValueSql("''''''")
                .HasColumnName("post_mime_type");
            entity.Property(e => e.PostModified)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("post_modified");
            entity.Property(e => e.PostModifiedGmt)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("post_modified_gmt");
            entity.Property(e => e.PostName)
                .IsRequired()
                .HasMaxLength(200)
                .HasDefaultValueSql("''''''")
                .HasColumnName("post_name");
            entity.Property(e => e.PostParent)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("post_parent");
            entity.Property(e => e.PostPassword)
                .IsRequired()
                .HasMaxLength(255)
                .HasDefaultValueSql("''''''")
                .HasColumnName("post_password");
            entity.Property(e => e.PostStatus)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValueSql("'''publish'''")
                .HasColumnName("post_status");
            entity.Property(e => e.PostTitle)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("post_title");
            entity.Property(e => e.PostType)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValueSql("'''post'''")
                .HasColumnName("post_type");
            entity.Property(e => e.ToPing)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("to_ping");
        });

        modelBuilder.Entity<WpPostmetum>(entity =>
        {
            entity.HasKey(e => e.MetaId).HasName("PRIMARY");

            entity.ToTable("wp_postmeta");

            entity.HasIndex(e => e.MetaKey, "meta_key");

            entity.HasIndex(e => e.PostId, "post_id");

            entity.Property(e => e.MetaId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("meta_id");
            entity.Property(e => e.MetaKey)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("meta_key");
            entity.Property(e => e.MetaValue)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("meta_value");
            entity.Property(e => e.PostId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("post_id");
        });

        modelBuilder.Entity<WpSnippet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("wp_snippets");

            entity.HasIndex(e => e.Active, "active");

            entity.HasIndex(e => e.Scope, "scope");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20)")
                .HasColumnName("id");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Code)
                .IsRequired()
                .HasColumnName("code");
            entity.Property(e => e.Description)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("modified");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("tinytext")
                .HasColumnName("name");
            entity.Property(e => e.Priority)
                .HasDefaultValueSql("'10'")
                .HasColumnType("smallint(6)")
                .HasColumnName("priority");
            entity.Property(e => e.Scope)
                .IsRequired()
                .HasMaxLength(15)
                .HasDefaultValueSql("'''global'''")
                .HasColumnName("scope");
            entity.Property(e => e.Tags)
                .IsRequired()
                .HasColumnName("tags");
        });

        modelBuilder.Entity<WpTerm>(entity =>
        {
            entity.HasKey(e => e.TermId).HasName("PRIMARY");

            entity.ToTable("wp_terms");

            entity.HasIndex(e => e.Name, "name");

            entity.HasIndex(e => e.Slug, "slug");

            entity.Property(e => e.TermId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("term_id");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200)
                .HasDefaultValueSql("''''''")
                .HasColumnName("name");
            entity.Property(e => e.Slug)
                .IsRequired()
                .HasMaxLength(200)
                .HasDefaultValueSql("''''''")
                .HasColumnName("slug");
            entity.Property(e => e.TermGroup)
                .HasColumnType("bigint(10)")
                .HasColumnName("term_group");
        });

        modelBuilder.Entity<WpTermRelationship>(entity =>
        {
            entity.HasKey(e => new { e.ObjectId, e.TermTaxonomyId }).HasName("PRIMARY");

            entity.ToTable("wp_term_relationships");

            entity.HasIndex(e => e.TermTaxonomyId, "term_taxonomy_id");

            entity.Property(e => e.ObjectId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("object_id");
            entity.Property(e => e.TermTaxonomyId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("term_taxonomy_id");
            entity.Property(e => e.TermOrder)
                .HasColumnType("int(11)")
                .HasColumnName("term_order");
        });

        modelBuilder.Entity<WpTermTaxonomy>(entity =>
        {
            entity.HasKey(e => e.TermTaxonomyId).HasName("PRIMARY");

            entity.ToTable("wp_term_taxonomy");

            entity.HasIndex(e => e.Taxonomy, "taxonomy");

            entity.HasIndex(e => new { e.TermId, e.Taxonomy }, "term_id_taxonomy").IsUnique();

            entity.Property(e => e.TermTaxonomyId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("term_taxonomy_id");
            entity.Property(e => e.Count)
                .HasColumnType("bigint(20)")
                .HasColumnName("count");
            entity.Property(e => e.Description)
                .IsRequired()
                .HasColumnName("description");
            entity.Property(e => e.Parent)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("parent");
            entity.Property(e => e.Taxonomy)
                .IsRequired()
                .HasMaxLength(32)
                .HasDefaultValueSql("''''''")
                .HasColumnName("taxonomy");
            entity.Property(e => e.TermId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("term_id");
        });

        modelBuilder.Entity<WpTermmetum>(entity =>
        {
            entity.HasKey(e => e.MetaId).HasName("PRIMARY");

            entity.ToTable("wp_termmeta");

            entity.HasIndex(e => e.MetaKey, "meta_key");

            entity.HasIndex(e => e.TermId, "term_id");

            entity.Property(e => e.MetaId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("meta_id");
            entity.Property(e => e.MetaKey)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("meta_key");
            entity.Property(e => e.MetaValue)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("meta_value");
            entity.Property(e => e.TermId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("term_id");
        });

        modelBuilder.Entity<WpTmTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("wp_tm_tasks");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Attempts)
                .HasDefaultValueSql("'0'")
                .HasColumnType("int(11)")
                .HasColumnName("attempts");
            entity.Property(e => e.ClassIdentifier)
                .HasMaxLength(300)
                .HasDefaultValueSql("'''0'''")
                .HasColumnName("class_identifier");
            entity.Property(e => e.Description)
                .HasMaxLength(300)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("description");
            entity.Property(e => e.LastLockedAt)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(20)")
                .HasColumnName("last_locked_at");
            entity.Property(e => e.Status)
                .HasMaxLength(300)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("status");
            entity.Property(e => e.TimeCreated)
                .HasDefaultValueSql("'current_timestamp()'")
                .HasColumnType("timestamp")
                .HasColumnName("time_created");
            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(300)
                .HasColumnName("type");
            entity.Property(e => e.UserId)
                .HasColumnType("bigint(20)")
                .HasColumnName("user_id");
        });

        modelBuilder.Entity<WpTmTaskmetum>(entity =>
        {
            entity.HasKey(e => e.MetaId).HasName("PRIMARY");

            entity.ToTable("wp_tm_taskmeta");

            entity.HasIndex(e => e.MetaKey, "meta_key");

            entity.HasIndex(e => e.TaskId, "task_id");

            entity.Property(e => e.MetaId)
                .HasColumnType("bigint(20)")
                .HasColumnName("meta_id");
            entity.Property(e => e.MetaKey)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("meta_key");
            entity.Property(e => e.MetaValue)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("meta_value");
            entity.Property(e => e.TaskId)
                .HasColumnType("bigint(20)")
                .HasColumnName("task_id");
        });

        modelBuilder.Entity<WpUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("wp_users");

            entity.HasIndex(e => e.UserEmail, "user_email");

            entity.HasIndex(e => e.UserLogin, "user_login_key");

            entity.HasIndex(e => e.UserNicename, "user_nicename");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ID");
            entity.Property(e => e.DisplayName)
                .IsRequired()
                .HasMaxLength(250)
                .HasDefaultValueSql("''''''")
                .HasColumnName("display_name");
            entity.Property(e => e.UserActivationKey)
                .IsRequired()
                .HasMaxLength(255)
                .HasDefaultValueSql("''''''")
                .HasColumnName("user_activation_key");
            entity.Property(e => e.UserEmail)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValueSql("''''''")
                .HasColumnName("user_email");
            entity.Property(e => e.UserLogin)
                .IsRequired()
                .HasMaxLength(60)
                .HasDefaultValueSql("''''''")
                .HasColumnName("user_login");
            entity.Property(e => e.UserNicename)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValueSql("''''''")
                .HasColumnName("user_nicename");
            entity.Property(e => e.UserPass)
                .IsRequired()
                .HasMaxLength(255)
                .HasDefaultValueSql("''''''")
                .HasColumnName("user_pass");
            entity.Property(e => e.UserRegistered)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("user_registered");
            entity.Property(e => e.UserStatus)
                .HasColumnType("int(11)")
                .HasColumnName("user_status");
            entity.Property(e => e.UserUrl)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValueSql("''''''")
                .HasColumnName("user_url");
        });

        modelBuilder.Entity<WpUsermetum>(entity =>
        {
            entity.HasKey(e => e.UmetaId).HasName("PRIMARY");

            entity.ToTable("wp_usermeta");

            entity.HasIndex(e => e.MetaKey, "meta_key");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.UmetaId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("umeta_id");
            entity.Property(e => e.MetaKey)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("meta_key");
            entity.Property(e => e.MetaValue)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("meta_value");
            entity.Property(e => e.UserId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("user_id");
        });

        modelBuilder.Entity<WpWcAdminNote>(entity =>
        {
            entity.HasKey(e => e.NoteId).HasName("PRIMARY");

            entity.ToTable("wp_wc_admin_notes");

            entity.Property(e => e.NoteId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("note_id");
            entity.Property(e => e.Content)
                .IsRequired()
                .HasColumnName("content");
            entity.Property(e => e.ContentData)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("content_data");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("date_created");
            entity.Property(e => e.DateReminder)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("datetime")
                .HasColumnName("date_reminder");
            entity.Property(e => e.Icon)
                .IsRequired()
                .HasMaxLength(200)
                .HasDefaultValueSql("'''info'''")
                .HasColumnName("icon");
            entity.Property(e => e.Image)
                .HasMaxLength(200)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("image");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.IsRead).HasColumnName("is_read");
            entity.Property(e => e.IsSnoozable).HasColumnName("is_snoozable");
            entity.Property(e => e.Layout)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValueSql("''''''")
                .HasColumnName("layout");
            entity.Property(e => e.Locale)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("locale");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Source)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("source");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .IsRequired()
                .HasColumnName("title");
            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("type");
        });

        modelBuilder.Entity<WpWcAdminNoteAction>(entity =>
        {
            entity.HasKey(e => e.ActionId).HasName("PRIMARY");

            entity.ToTable("wp_wc_admin_note_actions");

            entity.HasIndex(e => e.NoteId, "note_id");

            entity.Property(e => e.ActionId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("action_id");
            entity.Property(e => e.ActionedText)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("actioned_text");
            entity.Property(e => e.Label)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("label");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.NonceAction)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("nonce_action");
            entity.Property(e => e.NonceName)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("nonce_name");
            entity.Property(e => e.NoteId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("note_id");
            entity.Property(e => e.Query)
                .IsRequired()
                .HasColumnName("query");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("status");
        });

        modelBuilder.Entity<WpWcCategoryLookup>(entity =>
        {
            entity.HasKey(e => new { e.CategoryTreeId, e.CategoryId }).HasName("PRIMARY");

            entity.ToTable("wp_wc_category_lookup");

            entity.Property(e => e.CategoryTreeId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("category_tree_id");
            entity.Property(e => e.CategoryId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("category_id");
        });

        modelBuilder.Entity<WpWcCustomerLookup>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PRIMARY");

            entity.ToTable("wp_wc_customer_lookup");

            entity.HasIndex(e => e.Email, "email");

            entity.HasIndex(e => e.UserId, "user_id").IsUnique();

            entity.Property(e => e.CustomerId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("customer_id");
            entity.Property(e => e.City)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValueSql("''''''")
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .IsRequired()
                .HasMaxLength(2)
                .HasDefaultValueSql("''''''")
                .IsFixedLength()
                .HasColumnName("country");
            entity.Property(e => e.DateLastActive)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("timestamp")
                .HasColumnName("date_last_active");
            entity.Property(e => e.DateRegistered)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("timestamp")
                .HasColumnName("date_registered");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("last_name");
            entity.Property(e => e.Postcode)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValueSql("''''''")
                .HasColumnName("postcode");
            entity.Property(e => e.State)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValueSql("''''''")
                .HasColumnName("state");
            entity.Property(e => e.UserId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("user_id");
            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(60)
                .HasDefaultValueSql("''''''")
                .HasColumnName("username");
        });

        modelBuilder.Entity<WpWcDownloadLog>(entity =>
        {
            entity.HasKey(e => e.DownloadLogId).HasName("PRIMARY");

            entity.ToTable("wp_wc_download_log");

            entity.HasIndex(e => e.PermissionId, "permission_id");

            entity.HasIndex(e => e.Timestamp, "timestamp");

            entity.Property(e => e.DownloadLogId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("download_log_id");
            entity.Property(e => e.PermissionId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("permission_id");
            entity.Property(e => e.Timestamp)
                .HasColumnType("datetime")
                .HasColumnName("timestamp");
            entity.Property(e => e.UserId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("user_id");
            entity.Property(e => e.UserIpAddress)
                .HasMaxLength(100)
                .HasDefaultValueSql("''''''")
                .HasColumnName("user_ip_address");
        });

        modelBuilder.Entity<WpWcOrderCouponLookup>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.CouponId }).HasName("PRIMARY");

            entity.ToTable("wp_wc_order_coupon_lookup");

            entity.HasIndex(e => e.CouponId, "coupon_id");

            entity.HasIndex(e => e.DateCreated, "date_created");

            entity.Property(e => e.OrderId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("order_id");
            entity.Property(e => e.CouponId)
                .HasColumnType("bigint(20)")
                .HasColumnName("coupon_id");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("date_created");
            entity.Property(e => e.DiscountAmount).HasColumnName("discount_amount");
        });

        modelBuilder.Entity<WpWcOrderProductLookup>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PRIMARY");

            entity.ToTable("wp_wc_order_product_lookup");

            entity.HasIndex(e => e.CustomerId, "customer_id");

            entity.HasIndex(e => e.DateCreated, "date_created");

            entity.HasIndex(e => e.OrderId, "order_id");

            entity.HasIndex(e => e.ProductId, "product_id");

            entity.Property(e => e.OrderItemId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("order_item_id");
            entity.Property(e => e.CouponAmount).HasColumnName("coupon_amount");
            entity.Property(e => e.CustomerId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("customer_id");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("date_created");
            entity.Property(e => e.OrderId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("order_id");
            entity.Property(e => e.ProductGrossRevenue).HasColumnName("product_gross_revenue");
            entity.Property(e => e.ProductId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("product_id");
            entity.Property(e => e.ProductNetRevenue).HasColumnName("product_net_revenue");
            entity.Property(e => e.ProductQty)
                .HasColumnType("int(11)")
                .HasColumnName("product_qty");
            entity.Property(e => e.ShippingAmount).HasColumnName("shipping_amount");
            entity.Property(e => e.ShippingTaxAmount).HasColumnName("shipping_tax_amount");
            entity.Property(e => e.TaxAmount).HasColumnName("tax_amount");
            entity.Property(e => e.VariationId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("variation_id");
        });

        modelBuilder.Entity<WpWcOrderStat>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PRIMARY");

            entity.ToTable("wp_wc_order_stats");

            entity.HasIndex(e => e.CustomerId, "customer_id");

            entity.HasIndex(e => e.DateCreated, "date_created");

            entity.HasIndex(e => e.Status, "status");

            entity.Property(e => e.OrderId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("order_id");
            entity.Property(e => e.CustomerId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("customer_id");
            entity.Property(e => e.DateCompleted)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("date_completed");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("date_created");
            entity.Property(e => e.DateCreatedGmt)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("date_created_gmt");
            entity.Property(e => e.DatePaid)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("date_paid");
            entity.Property(e => e.NetTotal).HasColumnName("net_total");
            entity.Property(e => e.NumItemsSold)
                .HasColumnType("int(11)")
                .HasColumnName("num_items_sold");
            entity.Property(e => e.ParentId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("parent_id");
            entity.Property(e => e.ReturningCustomer)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("returning_customer");
            entity.Property(e => e.ShippingTotal).HasColumnName("shipping_total");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("status");
            entity.Property(e => e.TaxTotal).HasColumnName("tax_total");
            entity.Property(e => e.TotalSales).HasColumnName("total_sales");
        });

        modelBuilder.Entity<WpWcOrderTaxLookup>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.TaxRateId }).HasName("PRIMARY");

            entity.ToTable("wp_wc_order_tax_lookup");

            entity.HasIndex(e => e.DateCreated, "date_created");

            entity.HasIndex(e => e.TaxRateId, "tax_rate_id");

            entity.Property(e => e.OrderId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("order_id");
            entity.Property(e => e.TaxRateId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("tax_rate_id");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("date_created");
            entity.Property(e => e.OrderTax).HasColumnName("order_tax");
            entity.Property(e => e.ShippingTax).HasColumnName("shipping_tax");
            entity.Property(e => e.TotalTax).HasColumnName("total_tax");
        });

        modelBuilder.Entity<WpWcProductAttributesLookup>(entity =>
        {
            entity.HasKey(e => new { e.ProductOrParentId, e.TermId, e.ProductId, e.Taxonomy }).HasName("PRIMARY");

            entity.ToTable("wp_wc_product_attributes_lookup");

            entity.HasIndex(e => new { e.IsVariationAttribute, e.TermId }, "is_variation_attribute_term_id");

            entity.Property(e => e.ProductOrParentId)
                .HasColumnType("bigint(20)")
                .HasColumnName("product_or_parent_id");
            entity.Property(e => e.TermId)
                .HasColumnType("bigint(20)")
                .HasColumnName("term_id");
            entity.Property(e => e.ProductId)
                .HasColumnType("bigint(20)")
                .HasColumnName("product_id");
            entity.Property(e => e.Taxonomy)
                .HasMaxLength(32)
                .HasColumnName("taxonomy");
            entity.Property(e => e.InStock).HasColumnName("in_stock");
            entity.Property(e => e.IsVariationAttribute).HasColumnName("is_variation_attribute");
        });

        modelBuilder.Entity<WpWcProductDownloadDirectory>(entity =>
        {
            entity.HasKey(e => e.UrlId).HasName("PRIMARY");

            entity.ToTable("wp_wc_product_download_directories");

            entity.HasIndex(e => e.Url, "url");

            entity.Property(e => e.UrlId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("url_id");
            entity.Property(e => e.Enabled).HasColumnName("enabled");
            entity.Property(e => e.Url)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnName("url");
        });

        modelBuilder.Entity<WpWcProductMetaLookup>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PRIMARY");

            entity.ToTable("wp_wc_product_meta_lookup");

            entity.HasIndex(e => e.Downloadable, "downloadable");

            entity.HasIndex(e => new { e.MinPrice, e.MaxPrice }, "min_max_price");

            entity.HasIndex(e => e.Onsale, "onsale");

            entity.HasIndex(e => e.StockQuantity, "stock_quantity");

            entity.HasIndex(e => e.StockStatus, "stock_status");

            entity.HasIndex(e => e.Virtual, "virtual");

            entity.Property(e => e.ProductId)
                .HasColumnType("bigint(20)")
                .HasColumnName("product_id");
            entity.Property(e => e.AverageRating)
                .HasPrecision(3)
                .HasDefaultValueSql("'0.00'")
                .HasColumnName("average_rating");
            entity.Property(e => e.Downloadable)
                .HasDefaultValueSql("'0'")
                .HasColumnName("downloadable");
            entity.Property(e => e.MaxPrice)
                .HasPrecision(19, 4)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("max_price");
            entity.Property(e => e.MinPrice)
                .HasPrecision(19, 4)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("min_price");
            entity.Property(e => e.Onsale)
                .HasDefaultValueSql("'0'")
                .HasColumnName("onsale");
            entity.Property(e => e.RatingCount)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(20)")
                .HasColumnName("rating_count");
            entity.Property(e => e.Sku)
                .HasMaxLength(100)
                .HasDefaultValueSql("''''''")
                .HasColumnName("sku");
            entity.Property(e => e.StockQuantity)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("stock_quantity");
            entity.Property(e => e.StockStatus)
                .HasMaxLength(100)
                .HasDefaultValueSql("'''instock'''")
                .HasColumnName("stock_status");
            entity.Property(e => e.TaxClass)
                .HasMaxLength(100)
                .HasDefaultValueSql("''''''")
                .HasColumnName("tax_class");
            entity.Property(e => e.TaxStatus)
                .HasMaxLength(100)
                .HasDefaultValueSql("'''taxable'''")
                .HasColumnName("tax_status");
            entity.Property(e => e.TotalSales)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(20)")
                .HasColumnName("total_sales");
            entity.Property(e => e.Virtual)
                .HasDefaultValueSql("'0'")
                .HasColumnName("virtual");
        });

        modelBuilder.Entity<WpWcRateLimit>(entity =>
        {
            entity.HasKey(e => e.RateLimitId).HasName("PRIMARY");

            entity.ToTable("wp_wc_rate_limits");

            entity.HasIndex(e => e.RateLimitKey, "rate_limit_key").IsUnique();

            entity.Property(e => e.RateLimitId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("rate_limit_id");
            entity.Property(e => e.RateLimitExpiry)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("rate_limit_expiry");
            entity.Property(e => e.RateLimitKey)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("rate_limit_key");
            entity.Property(e => e.RateLimitRemaining)
                .HasColumnType("smallint(10)")
                .HasColumnName("rate_limit_remaining");
        });

        modelBuilder.Entity<WpWcReservedStock>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId }).HasName("PRIMARY");

            entity.ToTable("wp_wc_reserved_stock");

            entity.Property(e => e.OrderId)
                .HasColumnType("bigint(20)")
                .HasColumnName("order_id");
            entity.Property(e => e.ProductId)
                .HasColumnType("bigint(20)")
                .HasColumnName("product_id");
            entity.Property(e => e.Expires)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("expires");
            entity.Property(e => e.StockQuantity).HasColumnName("stock_quantity");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("timestamp");
        });

        modelBuilder.Entity<WpWcTaxRateClass>(entity =>
        {
            entity.HasKey(e => e.TaxRateClassId).HasName("PRIMARY");

            entity.ToTable("wp_wc_tax_rate_classes");

            entity.HasIndex(e => e.Slug, "slug").IsUnique();

            entity.Property(e => e.TaxRateClassId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("tax_rate_class_id");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200)
                .HasDefaultValueSql("''''''")
                .HasColumnName("name");
            entity.Property(e => e.Slug)
                .IsRequired()
                .HasMaxLength(200)
                .HasDefaultValueSql("''''''")
                .HasColumnName("slug");
        });

        modelBuilder.Entity<WpWcWebhook>(entity =>
        {
            entity.HasKey(e => e.WebhookId).HasName("PRIMARY");

            entity.ToTable("wp_wc_webhooks");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.WebhookId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("webhook_id");
            entity.Property(e => e.ApiVersion)
                .HasColumnType("smallint(4)")
                .HasColumnName("api_version");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("date_created");
            entity.Property(e => e.DateCreatedGmt)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("date_created_gmt");
            entity.Property(e => e.DateModified)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("date_modified");
            entity.Property(e => e.DateModifiedGmt)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("date_modified_gmt");
            entity.Property(e => e.DeliveryUrl)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("delivery_url");
            entity.Property(e => e.FailureCount)
                .HasColumnType("smallint(10)")
                .HasColumnName("failure_count");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("name");
            entity.Property(e => e.PendingDelivery).HasColumnName("pending_delivery");
            entity.Property(e => e.Secret)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("secret");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("status");
            entity.Property(e => e.Topic)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("topic");
            entity.Property(e => e.UserId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("user_id");
        });

        modelBuilder.Entity<WpWoocommerceApiKey>(entity =>
        {
            entity.HasKey(e => e.KeyId).HasName("PRIMARY");

            entity.ToTable("wp_woocommerce_api_keys");

            entity.HasIndex(e => e.ConsumerKey, "consumer_key");

            entity.HasIndex(e => e.ConsumerSecret, "consumer_secret");

            entity.Property(e => e.KeyId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("key_id");
            entity.Property(e => e.ConsumerKey)
                .IsRequired()
                .HasMaxLength(64)
                .IsFixedLength()
                .HasColumnName("consumer_key");
            entity.Property(e => e.ConsumerSecret)
                .IsRequired()
                .HasMaxLength(43)
                .IsFixedLength()
                .HasColumnName("consumer_secret");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("description");
            entity.Property(e => e.LastAccess)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("datetime")
                .HasColumnName("last_access");
            entity.Property(e => e.Nonces)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("nonces");
            entity.Property(e => e.Permissions)
                .IsRequired()
                .HasMaxLength(10)
                .HasColumnName("permissions");
            entity.Property(e => e.TruncatedKey)
                .IsRequired()
                .HasMaxLength(7)
                .IsFixedLength()
                .HasColumnName("truncated_key");
            entity.Property(e => e.UserId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("user_id");
        });

        modelBuilder.Entity<WpWoocommerceAttributeTaxonomy>(entity =>
        {
            entity.HasKey(e => e.AttributeId).HasName("PRIMARY");

            entity.ToTable("wp_woocommerce_attribute_taxonomies");

            entity.HasIndex(e => e.AttributeName, "attribute_name");

            entity.Property(e => e.AttributeId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("attribute_id");
            entity.Property(e => e.AttributeLabel)
                .HasMaxLength(200)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("attribute_label");
            entity.Property(e => e.AttributeName)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("attribute_name");
            entity.Property(e => e.AttributeOrderby)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("attribute_orderby");
            entity.Property(e => e.AttributePublic)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(1)")
                .HasColumnName("attribute_public");
            entity.Property(e => e.AttributeType)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("attribute_type");
        });

        modelBuilder.Entity<WpWoocommerceDownloadableProductPermission>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("PRIMARY");

            entity.ToTable("wp_woocommerce_downloadable_product_permissions");

            entity.HasIndex(e => new { e.ProductId, e.OrderId, e.OrderKey, e.DownloadId }, "download_order_key_product");

            entity.HasIndex(e => new { e.DownloadId, e.OrderId, e.ProductId }, "download_order_product");

            entity.HasIndex(e => e.OrderId, "order_id");

            entity.HasIndex(e => new { e.UserId, e.OrderId, e.DownloadsRemaining, e.AccessExpires }, "user_order_remaining_expires");

            entity.Property(e => e.PermissionId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("permission_id");
            entity.Property(e => e.AccessExpires)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("datetime")
                .HasColumnName("access_expires");
            entity.Property(e => e.AccessGranted)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("access_granted");
            entity.Property(e => e.DownloadCount)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("download_count");
            entity.Property(e => e.DownloadId)
                .IsRequired()
                .HasMaxLength(36)
                .HasColumnName("download_id");
            entity.Property(e => e.DownloadsRemaining)
                .HasMaxLength(9)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("downloads_remaining");
            entity.Property(e => e.OrderId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("order_id");
            entity.Property(e => e.OrderKey)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("order_key");
            entity.Property(e => e.ProductId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("product_id");
            entity.Property(e => e.UserEmail)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("user_email");
            entity.Property(e => e.UserId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("user_id");
        });

        modelBuilder.Entity<WpWoocommerceLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PRIMARY");

            entity.ToTable("wp_woocommerce_log");

            entity.HasIndex(e => e.Level, "level");

            entity.Property(e => e.LogId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("log_id");
            entity.Property(e => e.Context)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("context");
            entity.Property(e => e.Level)
                .HasColumnType("smallint(4)")
                .HasColumnName("level");
            entity.Property(e => e.Message)
                .IsRequired()
                .HasColumnName("message");
            entity.Property(e => e.Source)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("source");
            entity.Property(e => e.Timestamp)
                .HasColumnType("datetime")
                .HasColumnName("timestamp");
        });

        modelBuilder.Entity<WpWoocommerceOrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PRIMARY");

            entity.ToTable("wp_woocommerce_order_items");

            entity.HasIndex(e => e.OrderId, "order_id");

            entity.Property(e => e.OrderItemId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("order_item_id");
            entity.Property(e => e.OrderId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("order_id");
            entity.Property(e => e.OrderItemName)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("order_item_name");
            entity.Property(e => e.OrderItemType)
                .IsRequired()
                .HasMaxLength(200)
                .HasDefaultValueSql("''''''")
                .HasColumnName("order_item_type");
        });

        modelBuilder.Entity<WpWoocommerceOrderItemmetum>(entity =>
        {
            entity.HasKey(e => e.MetaId).HasName("PRIMARY");

            entity.ToTable("wp_woocommerce_order_itemmeta");

            entity.HasIndex(e => e.MetaKey, "meta_key");

            entity.HasIndex(e => e.OrderItemId, "order_item_id");

            entity.Property(e => e.MetaId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("meta_id");
            entity.Property(e => e.MetaKey)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("meta_key");
            entity.Property(e => e.MetaValue)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("meta_value");
            entity.Property(e => e.OrderItemId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("order_item_id");
        });

        modelBuilder.Entity<WpWoocommercePaymentToken>(entity =>
        {
            entity.HasKey(e => e.TokenId).HasName("PRIMARY");

            entity.ToTable("wp_woocommerce_payment_tokens");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.TokenId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("token_id");
            entity.Property(e => e.GatewayId)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("gateway_id");
            entity.Property(e => e.IsDefault).HasColumnName("is_default");
            entity.Property(e => e.Token)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("token");
            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("type");
            entity.Property(e => e.UserId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("user_id");
        });

        modelBuilder.Entity<WpWoocommercePaymentTokenmetum>(entity =>
        {
            entity.HasKey(e => e.MetaId).HasName("PRIMARY");

            entity.ToTable("wp_woocommerce_payment_tokenmeta");

            entity.HasIndex(e => e.MetaKey, "meta_key");

            entity.HasIndex(e => e.PaymentTokenId, "payment_token_id");

            entity.Property(e => e.MetaId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("meta_id");
            entity.Property(e => e.MetaKey)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("meta_key");
            entity.Property(e => e.MetaValue)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("meta_value");
            entity.Property(e => e.PaymentTokenId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("payment_token_id");
        });

        modelBuilder.Entity<WpWoocommerceSession>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("PRIMARY");

            entity.ToTable("wp_woocommerce_sessions");

            entity.HasIndex(e => e.SessionKey, "session_key").IsUnique();

            entity.Property(e => e.SessionId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("session_id");
            entity.Property(e => e.SessionExpiry)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("session_expiry");
            entity.Property(e => e.SessionKey)
                .IsRequired()
                .HasMaxLength(32)
                .IsFixedLength()
                .HasColumnName("session_key");
            entity.Property(e => e.SessionValue)
                .IsRequired()
                .HasColumnName("session_value");
        });

        modelBuilder.Entity<WpWoocommerceShippingZone>(entity =>
        {
            entity.HasKey(e => e.ZoneId).HasName("PRIMARY");

            entity.ToTable("wp_woocommerce_shipping_zones");

            entity.Property(e => e.ZoneId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("zone_id");
            entity.Property(e => e.ZoneName)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("zone_name");
            entity.Property(e => e.ZoneOrder)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("zone_order");
        });

        modelBuilder.Entity<WpWoocommerceShippingZoneLocation>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("PRIMARY");

            entity.ToTable("wp_woocommerce_shipping_zone_locations");

            entity.HasIndex(e => e.LocationId, "location_id");

            entity.HasIndex(e => new { e.LocationType, e.LocationCode }, "location_type_code");

            entity.Property(e => e.LocationId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("location_id");
            entity.Property(e => e.LocationCode)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("location_code");
            entity.Property(e => e.LocationType)
                .IsRequired()
                .HasMaxLength(40)
                .HasColumnName("location_type");
            entity.Property(e => e.ZoneId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("zone_id");
        });

        modelBuilder.Entity<WpWoocommerceShippingZoneMethod>(entity =>
        {
            entity.HasKey(e => e.InstanceId).HasName("PRIMARY");

            entity.ToTable("wp_woocommerce_shipping_zone_methods");

            entity.Property(e => e.InstanceId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("instance_id");
            entity.Property(e => e.IsEnabled)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("is_enabled");
            entity.Property(e => e.MethodId)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("method_id");
            entity.Property(e => e.MethodOrder)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("method_order");
            entity.Property(e => e.ZoneId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("zone_id");
        });

        modelBuilder.Entity<WpWoocommerceTaxRate>(entity =>
        {
            entity.HasKey(e => e.TaxRateId).HasName("PRIMARY");

            entity.ToTable("wp_woocommerce_tax_rates");

            entity.HasIndex(e => e.TaxRateClass, "tax_rate_class");

            entity.HasIndex(e => e.TaxRateCountry, "tax_rate_country");

            entity.HasIndex(e => e.TaxRatePriority, "tax_rate_priority");

            entity.HasIndex(e => e.TaxRateState, "tax_rate_state");

            entity.Property(e => e.TaxRateId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("tax_rate_id");
            entity.Property(e => e.TaxRate)
                .IsRequired()
                .HasMaxLength(8)
                .HasDefaultValueSql("''''''")
                .HasColumnName("tax_rate");
            entity.Property(e => e.TaxRateClass)
                .IsRequired()
                .HasMaxLength(200)
                .HasDefaultValueSql("''''''")
                .HasColumnName("tax_rate_class");
            entity.Property(e => e.TaxRateCompound)
                .HasColumnType("int(1)")
                .HasColumnName("tax_rate_compound");
            entity.Property(e => e.TaxRateCountry)
                .IsRequired()
                .HasMaxLength(2)
                .HasDefaultValueSql("''''''")
                .HasColumnName("tax_rate_country");
            entity.Property(e => e.TaxRateName)
                .IsRequired()
                .HasMaxLength(200)
                .HasDefaultValueSql("''''''")
                .HasColumnName("tax_rate_name");
            entity.Property(e => e.TaxRateOrder)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("tax_rate_order");
            entity.Property(e => e.TaxRatePriority)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("tax_rate_priority");
            entity.Property(e => e.TaxRateShipping)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(1)")
                .HasColumnName("tax_rate_shipping");
            entity.Property(e => e.TaxRateState)
                .IsRequired()
                .HasMaxLength(200)
                .HasDefaultValueSql("''''''")
                .HasColumnName("tax_rate_state");
        });

        modelBuilder.Entity<WpWoocommerceTaxRateLocation>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("PRIMARY");

            entity.ToTable("wp_woocommerce_tax_rate_locations");

            entity.HasIndex(e => new { e.LocationType, e.LocationCode }, "location_type_code");

            entity.HasIndex(e => e.TaxRateId, "tax_rate_id");

            entity.Property(e => e.LocationId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("location_id");
            entity.Property(e => e.LocationCode)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("location_code");
            entity.Property(e => e.LocationType)
                .IsRequired()
                .HasMaxLength(40)
                .HasColumnName("location_type");
            entity.Property(e => e.TaxRateId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("tax_rate_id");
        });

        modelBuilder.Entity<WpWssLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("wp_wss_log");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Data)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("data");
            entity.Property(e => e.HasError)
                .HasDefaultValueSql("'0'")
                .HasColumnType("smallint(1)")
                .HasColumnName("has_error");
            entity.Property(e => e.Message)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("message");
            entity.Property(e => e.ProductId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("product_id");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasDefaultValueSql("''''''")
                .HasColumnName("type");
        });

        modelBuilder.Entity<WpXsgSitemapMetum>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("wp_xsg_sitemap_meta", tb => tb.HasComment("generatated by XmlSitemapGenerator.org"));

            entity.HasIndex(e => new { e.ItemId, e.ItemType }, "idx_xsg_sitemap_meta_ItemId_ItemType").IsUnique();

            entity.Property(e => e.Exclude)
                .HasDefaultValueSql("'0'")
                .HasColumnType("int(11)")
                .HasColumnName("exclude");
            entity.Property(e => e.Frequency)
                .HasDefaultValueSql("'0'")
                .HasColumnType("int(11)")
                .HasColumnName("frequency");
            entity.Property(e => e.Inherit)
                .HasDefaultValueSql("'0'")
                .HasColumnType("int(11)")
                .HasColumnName("inherit");
            entity.Property(e => e.ItemId)
                .HasDefaultValueSql("'0'")
                .HasColumnType("int(11)")
                .HasColumnName("itemId");
            entity.Property(e => e.ItemType)
                .HasMaxLength(8)
                .HasDefaultValueSql("''''''")
                .HasColumnName("itemType");
            entity.Property(e => e.News)
                .HasColumnType("int(11)")
                .HasColumnName("news");
            entity.Property(e => e.Priority)
                .HasDefaultValueSql("'0'")
                .HasColumnType("int(11)")
                .HasColumnName("priority");
        });

        modelBuilder.Entity<WpYithWcwl>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("wp_yith_wcwl");

            entity.HasIndex(e => e.ProdId, "prod_id");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20)")
                .HasColumnName("ID");
            entity.Property(e => e.Dateadded)
                .HasDefaultValueSql("'current_timestamp()'")
                .HasColumnType("timestamp")
                .HasColumnName("dateadded");
            entity.Property(e => e.OnSale)
                .HasColumnType("tinyint(4)")
                .HasColumnName("on_sale");
            entity.Property(e => e.OriginalCurrency)
                .HasMaxLength(3)
                .HasDefaultValueSql("'NULL'")
                .IsFixedLength()
                .HasColumnName("original_currency");
            entity.Property(e => e.OriginalPrice)
                .HasPrecision(9, 3)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("original_price");
            entity.Property(e => e.Position)
                .HasDefaultValueSql("'0'")
                .HasColumnType("int(11)")
                .HasColumnName("position");
            entity.Property(e => e.ProdId)
                .HasColumnType("bigint(20)")
                .HasColumnName("prod_id");
            entity.Property(e => e.Quantity)
                .HasColumnType("int(11)")
                .HasColumnName("quantity");
            entity.Property(e => e.UserId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("bigint(20)")
                .HasColumnName("user_id");
            entity.Property(e => e.WishlistId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("bigint(20)")
                .HasColumnName("wishlist_id");
        });

        modelBuilder.Entity<WpYithWcwlList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("wp_yith_wcwl_lists");

            entity.HasIndex(e => e.WishlistSlug, "wishlist_slug");

            entity.HasIndex(e => e.WishlistToken, "wishlist_token").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20)")
                .HasColumnName("ID");
            entity.Property(e => e.Dateadded)
                .HasDefaultValueSql("'current_timestamp()'")
                .HasColumnType("timestamp")
                .HasColumnName("dateadded");
            entity.Property(e => e.Expiration)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("timestamp")
                .HasColumnName("expiration");
            entity.Property(e => e.IsDefault).HasColumnName("is_default");
            entity.Property(e => e.SessionId)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("session_id");
            entity.Property(e => e.UserId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("bigint(20)")
                .HasColumnName("user_id");
            entity.Property(e => e.WishlistName)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("wishlist_name");
            entity.Property(e => e.WishlistPrivacy).HasColumnName("wishlist_privacy");
            entity.Property(e => e.WishlistSlug)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("wishlist_slug");
            entity.Property(e => e.WishlistToken)
                .IsRequired()
                .HasMaxLength(64)
                .HasColumnName("wishlist_token");
        });

        modelBuilder.Entity<WpYoastIndexable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("wp_yoast_indexable");

            entity.HasIndex(e => new { e.ObjectId, e.ObjectType }, "object_id_and_type");

            entity.HasIndex(e => new { e.ObjectType, e.ObjectSubType }, "object_type_and_sub_type");

            entity.HasIndex(e => new { e.PermalinkHash, e.ObjectType }, "permalink_hash_and_object_type");

            entity.HasIndex(e => new { e.ProminentWordsVersion, e.ObjectType, e.ObjectSubType, e.PostStatus }, "prominent_words");

            entity.HasIndex(e => new { e.ObjectPublishedAt, e.IsRobotsNoindex, e.ObjectType, e.ObjectSubType }, "published_sitemap_index");

            entity.HasIndex(e => new { e.PostParent, e.ObjectType, e.PostStatus, e.ObjectId }, "subpages");

            entity.Property(e => e.Id)
                .HasColumnType("int(11) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.AuthorId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("bigint(20)")
                .HasColumnName("author_id");
            entity.Property(e => e.BlogId)
                .HasDefaultValueSql("'1'")
                .HasColumnType("bigint(20)")
                .HasColumnName("blog_id");
            entity.Property(e => e.BreadcrumbTitle)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("breadcrumb_title");
            entity.Property(e => e.Canonical)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("canonical");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("mediumtext")
                .HasColumnName("description");
            entity.Property(e => e.EstimatedReadingTimeMinutes)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11)")
                .HasColumnName("estimated_reading_time_minutes");
            entity.Property(e => e.HasAncestors)
                .HasDefaultValueSql("'0'")
                .HasColumnName("has_ancestors");
            entity.Property(e => e.HasPublicPosts)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("has_public_posts");
            entity.Property(e => e.IncomingLinkCount)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11)")
                .HasColumnName("incoming_link_count");
            entity.Property(e => e.IsCornerstone)
                .HasDefaultValueSql("'0'")
                .HasColumnName("is_cornerstone");
            entity.Property(e => e.IsProtected)
                .HasDefaultValueSql("'0'")
                .HasColumnName("is_protected");
            entity.Property(e => e.IsPublic)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("is_public");
            entity.Property(e => e.IsRobotsNoarchive)
                .HasDefaultValueSql("'0'")
                .HasColumnName("is_robots_noarchive");
            entity.Property(e => e.IsRobotsNofollow)
                .HasDefaultValueSql("'0'")
                .HasColumnName("is_robots_nofollow");
            entity.Property(e => e.IsRobotsNoimageindex)
                .HasDefaultValueSql("'0'")
                .HasColumnName("is_robots_noimageindex");
            entity.Property(e => e.IsRobotsNoindex)
                .HasDefaultValueSql("'0'")
                .HasColumnName("is_robots_noindex");
            entity.Property(e => e.IsRobotsNosnippet)
                .HasDefaultValueSql("'0'")
                .HasColumnName("is_robots_nosnippet");
            entity.Property(e => e.Language)
                .HasMaxLength(32)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("language");
            entity.Property(e => e.LinkCount)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11)")
                .HasColumnName("link_count");
            entity.Property(e => e.NumberOfPages)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11) unsigned")
                .HasColumnName("number_of_pages");
            entity.Property(e => e.ObjectId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("bigint(20)")
                .HasColumnName("object_id");
            entity.Property(e => e.ObjectLastModified)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("datetime")
                .HasColumnName("object_last_modified");
            entity.Property(e => e.ObjectPublishedAt)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("datetime")
                .HasColumnName("object_published_at");
            entity.Property(e => e.ObjectSubType)
                .HasMaxLength(32)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("object_sub_type");
            entity.Property(e => e.ObjectType)
                .IsRequired()
                .HasMaxLength(32)
                .HasColumnName("object_type");
            entity.Property(e => e.OpenGraphDescription)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("open_graph_description");
            entity.Property(e => e.OpenGraphImage)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("open_graph_image");
            entity.Property(e => e.OpenGraphImageId)
                .HasMaxLength(191)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("open_graph_image_id");
            entity.Property(e => e.OpenGraphImageMeta)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("mediumtext")
                .HasColumnName("open_graph_image_meta");
            entity.Property(e => e.OpenGraphImageSource)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("open_graph_image_source");
            entity.Property(e => e.OpenGraphTitle)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("open_graph_title");
            entity.Property(e => e.Permalink)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("permalink");
            entity.Property(e => e.PermalinkHash)
                .HasMaxLength(40)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("permalink_hash");
            entity.Property(e => e.PostParent)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("bigint(20)")
                .HasColumnName("post_parent");
            entity.Property(e => e.PostStatus)
                .HasMaxLength(20)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("post_status");
            entity.Property(e => e.PrimaryFocusKeyword)
                .HasMaxLength(191)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("primary_focus_keyword");
            entity.Property(e => e.PrimaryFocusKeywordScore)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(3)")
                .HasColumnName("primary_focus_keyword_score");
            entity.Property(e => e.ProminentWordsVersion)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11) unsigned")
                .HasColumnName("prominent_words_version");
            entity.Property(e => e.ReadabilityScore)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(3)")
                .HasColumnName("readability_score");
            entity.Property(e => e.Region)
                .HasMaxLength(32)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("region");
            entity.Property(e => e.SchemaArticleType)
                .HasMaxLength(64)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("schema_article_type");
            entity.Property(e => e.SchemaPageType)
                .HasMaxLength(64)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("schema_page_type");
            entity.Property(e => e.Title)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("title");
            entity.Property(e => e.TwitterDescription)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("twitter_description");
            entity.Property(e => e.TwitterImage)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("twitter_image");
            entity.Property(e => e.TwitterImageId)
                .HasMaxLength(191)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("twitter_image_id");
            entity.Property(e => e.TwitterImageSource)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("twitter_image_source");
            entity.Property(e => e.TwitterTitle)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("twitter_title");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("'current_timestamp()'")
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.Version)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(11)")
                .HasColumnName("version");
        });

        modelBuilder.Entity<WpYoastIndexableHierarchy>(entity =>
        {
            entity.HasKey(e => new { e.IndexableId, e.AncestorId }).HasName("PRIMARY");

            entity.ToTable("wp_yoast_indexable_hierarchy");

            entity.HasIndex(e => e.AncestorId, "ancestor_id");

            entity.HasIndex(e => e.Depth, "depth");

            entity.HasIndex(e => e.IndexableId, "indexable_id");

            entity.Property(e => e.IndexableId)
                .HasColumnType("int(11) unsigned")
                .HasColumnName("indexable_id");
            entity.Property(e => e.AncestorId)
                .HasColumnType("int(11) unsigned")
                .HasColumnName("ancestor_id");
            entity.Property(e => e.BlogId)
                .HasDefaultValueSql("'1'")
                .HasColumnType("bigint(20)")
                .HasColumnName("blog_id");
            entity.Property(e => e.Depth)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11) unsigned")
                .HasColumnName("depth");
        });

        modelBuilder.Entity<WpYoastMigration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("wp_yoast_migrations");

            entity.HasIndex(e => e.Version, "wp_yoast_migrations_version").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Version)
                .HasMaxLength(191)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("version");
        });

        modelBuilder.Entity<WpYoastPrimaryTerm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("wp_yoast_primary_term");

            entity.HasIndex(e => new { e.PostId, e.Taxonomy }, "post_taxonomy");

            entity.HasIndex(e => new { e.PostId, e.TermId }, "post_term");

            entity.Property(e => e.Id)
                .HasColumnType("int(11) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.BlogId)
                .HasDefaultValueSql("'1'")
                .HasColumnType("bigint(20)")
                .HasColumnName("blog_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.PostId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("bigint(20)")
                .HasColumnName("post_id");
            entity.Property(e => e.Taxonomy)
                .IsRequired()
                .HasMaxLength(32)
                .HasColumnName("taxonomy");
            entity.Property(e => e.TermId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("bigint(20)")
                .HasColumnName("term_id");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("'current_timestamp()'")
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<WpYoastSeoLink>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("wp_yoast_seo_links");

            entity.HasIndex(e => new { e.IndexableId, e.Type }, "indexable_link_direction");

            entity.HasIndex(e => new { e.PostId, e.Type }, "link_direction");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Height)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11) unsigned")
                .HasColumnName("height");
            entity.Property(e => e.IndexableId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11) unsigned")
                .HasColumnName("indexable_id");
            entity.Property(e => e.Language)
                .HasMaxLength(32)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("language");
            entity.Property(e => e.PostId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("post_id");
            entity.Property(e => e.Region)
                .HasMaxLength(32)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("region");
            entity.Property(e => e.Size)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11) unsigned")
                .HasColumnName("size");
            entity.Property(e => e.TargetIndexableId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11) unsigned")
                .HasColumnName("target_indexable_id");
            entity.Property(e => e.TargetPostId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("target_post_id");
            entity.Property(e => e.Type)
                .HasMaxLength(8)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("type");
            entity.Property(e => e.Url)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("url");
            entity.Property(e => e.Width)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11) unsigned")
                .HasColumnName("width");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
