CREATE VIEW [dbo].[uv_HealthStatusWeight]
	AS 
    SELECT Id, 
   
       --calculate Tally start for the RunningTotal from the previous HealthStatus
               LAG(BaseRows.RunningTotal,1,0) OVER (ORDER BY Id) AS TallyStart,
 
       --then the end is the running total + current probability (or # of Tallys in set)
               BaseRows.EndingWeightFactorBase + 
                    LAG(BaseRows.RunningTotal,1,0) OVER (ORDER BY Id) -1 AS TallyEnd
    FROM (
           SELECT Id, Weight AS EndingWeightFactorBase,
                  --this provides the value for the start and end seperated 
                  SUM(HealthStatus.Weight) OVER (ORDER BY Id) AS RunningTotal
    
    FROM HealthStatus 
        Where HealthStatus.Id > 0 -- Status 0 is no status
	    AND HealthStatus.IsDeleted = 0
      ) AS BaseRows
    ;
    GO
