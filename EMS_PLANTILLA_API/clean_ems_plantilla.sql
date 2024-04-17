SELECT
	es.id  AS 'Trans id' 	
	, CONCAT(og.code, '- ', og.description) AS 'ORG GROUP'
	, region_org.description AS  'PARENT ORG'
	, CONCAT(emp.firstname , ' ', emp.lastname) AS 'EMPLOYEE NAME'
	, CONCAT(pos.code, '- ', pos.title) AS 'POSITION'
	,es.pdate_from AS 'DATE FROM'
	,es.pdate_to AS 'DATE TO'
	,es.created_date AS 'CREATED DATE'
	, es.status AS 'STATUS'
	-- ,kpi.id AS 'KPI ID'
	,kg.name AS 'KRA GROUP'
	,kpi.code AS 'KPI CODE'
	, kpi.name AS 'KPI NAME'
	, kpi.description AS 'KPI DESCRIPTION'
	, kpi.guidelines AS 'KPI GUIDELINES'
	, IFNULL(esd.kpi_weight, 0) AS 'WEIGHT'
	, IFNULL(esd.kpi_target, 0) AS 'TARGET'
	, IFNULL(esd.kpi_actual, 0) AS 'ACTUAL'
	, IFNULL(esd.kpi_score, 0) AS 'RATE'
	, IFNULL(esd.kpi_score, 0) * IFNULL(esd.kpi_weight, 0) AS 'TOTAL'
	, IFNULL(
		(SELECT code FROM rating_table WHERE (IFNULL(esd.kpi_score, 0) * 100) BETWEEN IFNULL(min_score,0) AND IFNULL(max_score,0)), 'N/A') AS 'GRADE'
	,  kpi.source_type  AS 'SOURCE TYPE'
	,  AS 'RATER'
	FROM trans_employee_score_details esd
	JOIN trans_employee_score es ON es.id = esd.trans_id
	LEFT JOIN ems_plantilla.employee emp ON emp.id = es.employee_id
	LEFT JOIN ems_plantilla.position pos ON pos.id = es.position_id
	LEFT JOIN ems_plantilla.org_group og ON og.id = emp.org_group_id
	LEFT JOIN ems_plantilla.org_group AS cluster_org ON og.parent_org_id = cluster_org.id
	LEFT JOIN ems_plantilla.org_group AS area_org ON cluster_org.parent_org_id = area_org.id
	LEFT JOIN ems_plantilla.org_group AS region_org ON area_org.parent_org_id = region_org.id
	JOIN kpi kpi ON esd.kpi_id = kpi.id
	JOIN kra_group kg ON kpi.kra_group = kg.id 
	WHERE  
	SUBSTRING(es.pdate_to,1,4) = '2023' AND SUBSTRING(es.created_date,1,4) = '2024'
AND (region_org.id = 4 OR og.id IN (2579,2525,2523,2517))
;

