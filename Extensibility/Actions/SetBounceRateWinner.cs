using Sitecore.ContentTesting;
using Sitecore.ContentTesting.Helpers;
using Sitecore.ContentTesting.Model;
using Sitecore.ContentTesting.Models;
using Sitecore.ContentTesting.Reports;
using Sitecore.ContentTesting.Rules;
using Sitecore.ContentTesting.Model.Extensions;
using System.Linq;
using Sitecore.ContentTesting.Pipelines.StopTest;
using Sitecore.ContentTesting.Pipelines;
using System.Collections.Generic;
using System;

namespace SitecoreDmsTestAutoWinnerCustomization.Extensibility.Actions
{

    public class SetBounceRateWinner<T> : Sitecore.Rules.Actions.RuleAction<T> where T : Sitecore.Rules.RuleContext
    {
        public override void Apply(T ruleContext)
        {
            FinishTestRuleContext finishTestRuleContext = ruleContext as FinishTestRuleContext;

            if (finishTestRuleContext != null)
            {
                ITestConfiguration configuration = finishTestRuleContext.Configuration;
                IEnumerable<TestExperience> testExperience = configuration.Experiences;
                IContentTestPerformance performanceForTest = finishTestRuleContext.ContentTestPerformanceFactory.GetPerformanceForTest(configuration);

                StopTestArgs stopTestArgs = new StopTestArgs();
                stopTestArgs.Configuration = finishTestRuleContext.Configuration;

                List<float> list = new List<float>();
                List<TestExperience> listTestExperience = new List<TestExperience>();

                foreach (var experience in testExperience)
                {

                    SiteStatistics siteStatistics = performanceForTest.GetExperienceSiteStatistics(experience.Combination);
                    list.Add(siteStatistics.BounceRate);
                    listTestExperience.Add(experience);
                }

                if (list.Count != 0)
                {
                    int indexOfMinBounceRate = GetIndexOfLowestBounceRate(list);

                    if (indexOfMinBounceRate != 0)
                    {
                        TestCombination testCombination = new TestCombination(listTestExperience[indexOfMinBounceRate].Combination, configuration.TestSet);

                        if (testCombination == null)
                        {
                            return;
                        }
                        using (new Sitecore.Data.Items.EditContext(configuration.TestDefinitionItem))
                        {
                            configuration.TestDefinitionItem.WinnerCombination = testCombination.Combination.MultiplexToString("-");
                        }
                        Sitecore.Data.Items.Item item = configuration.TestDefinitionItem;
                        Sitecore.Data.Database[] targets = PublishingHelper.GetTargets(item).ToArray<Sitecore.Data.Database>();
                        Sitecore.Publishing.PublishManager.PublishItem(item, targets, item.Languages, true, true);
                        stopTestArgs.Combination = testCombination;
                        SettingsDependantPipeline<StopTestPipeline, StopTestArgs>.Instance.Run(stopTestArgs);

                    }
                }

            }

        }

        protected int GetIndexOfLowestBounceRate(List<float> list)
        {
            int index = 0;

            float min = list[0];


            for (int i = 1; i < list.Count; ++i)
            {
                if (list[i] < min)
                {
                    min = list[i];
                    index = i;
                }
            }
            return index;
        }
    }
}