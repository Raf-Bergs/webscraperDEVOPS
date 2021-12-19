using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Threading;

namespace webScraper
{
    class Program
    {
        static void Main(string[] args)
        {


            Console.WriteLine("which Site?\nY - Youtube\nI - indeed\nV - IMVU");
            var selection = Console.ReadLine();

            if (selection.ToUpper() == "Y")
            { 

            Console.WriteLine("enter your keyword");
            var keyword = Console.ReadLine();

                // go to youtube
             IWebDriver driver = new ChromeDriver();
             driver.Navigate().GoToUrl("https://www.youtube.com/");
            Thread.Sleep(3000);
                // close popup
            driver.FindElement(By.XPath("/html/body/ytd-app/ytd-consent-bump-v2-lightbox/tp-yt-paper-dialog/div[4]/div[2]/div[5]/div[2]/ytd-button-renderer[2]/a/tp-yt-paper-button")).Click();
                // search bar zoeken
            var search = driver.FindElement(By.XPath("/html/body/ytd-app/div/div/ytd-masthead/div[3]/div[2]/ytd-searchbox/form/div[1]/div[1]/input"));
            Thread.Sleep(1000);
            // add keywords to search
            search.SendKeys(keyword);
            search.Submit();
            // add filters
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("#container > ytd-toggle-button-renderer")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("#label > yt-formatted-string")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("#container > ytd-toggle-button-renderer")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("/html/body/ytd-app/div/ytd-page-manager/ytd-search/div[1]/ytd-two-column-search-results-renderer/div/ytd-section-list-renderer/div[1]/div[2]/ytd-search-sub-menu-renderer/div[1]/iron-collapse/div/ytd-search-filter-group-renderer[5]/ytd-search-filter-renderer[2]/a")).Click();
            Thread.Sleep(2000);
            //keeps everything clean looking
            Console.WriteLine("\n");
            Console.WriteLine("Videos: ");
            //add seperator between searches in file
            youtubeFile("------------------", keyword, "------------------", "", "youtube.txt");

                for (int i = 1; i <= 5; i++)
            {
            // get video title
            var title = driver.FindElement(By.XPath("/html/body/ytd-app/div/ytd-page-manager/ytd-search/div[1]/ytd-two-column-search-results-renderer/div/ytd-section-list-renderer/div[2]/ytd-item-section-renderer/div[3]/ytd-video-renderer["+ i +"]/div[1]/div/div[1]/div/h3/a"));
            // get views
            var views = driver.FindElement(By.XPath("/html/body/ytd-app/div/ytd-page-manager/ytd-search/div[1]/ytd-two-column-search-results-renderer/div/ytd-section-list-renderer/div[2]/ytd-item-section-renderer/div[3]/ytd-video-renderer[" + i + "]/div[1]/div/div[1]/ytd-video-meta-block/div[1]/div[2]/span[1]"));
            // get uploader
            var uploader = driver.FindElement(By.XPath("/html/body/ytd-app/div/ytd-page-manager/ytd-search/div[1]/ytd-two-column-search-results-renderer/div/ytd-section-list-renderer/div[2]/ytd-item-section-renderer/div[3]/ytd-video-renderer[" + i + "]/div[1]/div/div[2]/ytd-channel-name/div/div/yt-formatted-string/a"));
            var link = title.GetAttribute("href");
            // print to console
            Console.WriteLine("Title: "+title.Text);
            Console.WriteLine("Views: "+views.Text);
            Console.WriteLine("Uploader: "+uploader.Text);
            Console.WriteLine("Link: "+link);
            Console.WriteLine("\n");
            //write to file in CSV format
            youtubeFile(title.Text, views.Text, uploader.Text, link, "youtube.txt");
            }
            }

            if (selection.ToUpper() == "I")
            {

                Console.WriteLine("enter your keyword");
                var keyword = Console.ReadLine();
                Console.WriteLine("enter your city or postal code");
                var city = Console.ReadLine();

                IWebDriver driver = new ChromeDriver();

                Actions actions = new Actions(driver);

                driver.Navigate().GoToUrl("https://be.indeed.com/");
                Thread.Sleep(2000);
                var search = driver.FindElement(By.CssSelector("#text-input-what"));
                var citySearch = driver.FindElement(By.CssSelector("#text-input-where"));
                search.SendKeys(keyword);
                citySearch.SendKeys(city);
                search.Submit();
                Thread.Sleep(2000);
                driver.FindElement(By.CssSelector("#filter-dateposted")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.CssSelector("#filter-dateposted-menu > li:nth-child(2) > a")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.CssSelector("#popover-x > button")).Click();
                Thread.Sleep(2000);
                var bottom = driver.FindElement(By.CssSelector("#gnav-footer-container > div > footer"));
                actions.MoveToElement(bottom);
                actions.Perform();
                var jobs = driver.FindElements(By.CssSelector("* > div.slider_container > div > div.slider_item > div table.jobCard_mainContent > tbody > tr > td"));
                Console.WriteLine("\n");
                indeedFile(keyword, "-----", city + "------------------------------", "indeed.txt");
                foreach (var job in jobs)
                {
                    var jobtitle = job.FindElement(By.CssSelector("div.heading4.color-text-primary.singleLineTitle.tapItem-gutter > h2 > span"));
                    var jobLocation = job.FindElement(By.CssSelector("div.heading6.company_location.tapItem-gutter > pre > div.companyLocation"));
                    var employer = job.FindElement(By.CssSelector("div.heading6.company_location.tapItem-gutter > pre > span.companyName"));
                    // I know where I can find the link but can't find a way to print it
                    Console.WriteLine("Title: "+jobtitle.Text);
                    Console.WriteLine("Location: "+jobLocation.Text);
                    Console.WriteLine("Employer: "+employer.Text);
                    Console.WriteLine("Link: ");
                    Console.WriteLine("\n");
                    indeedFile(jobtitle.Text, jobLocation.Text, employer.Text, "indeed.txt");
                }



            }

            if (selection.ToUpper() == "V")
            {

                Console.WriteLine("enter the username");
                var username = Console.ReadLine();

                IWebDriver driver = new ChromeDriver();
                driver.Navigate().GoToUrl("https://avatars.imvu.com/"+username);
                Thread.Sleep(5000);
                var img = driver.FindElement(By.XPath("//*[@id=\"send_message_link\"]"));
                string link = img.GetAttribute("href");
                string id = link.Substring(74);
                string api = "https://api.imvu.com/user/user-" + id;
                Console.WriteLine("\n"+link.Length);
                imvufile("----------",username+"----------" , "imvu.txt");
                imvufile(id, api, "imvu.txt");
                driver.Navigate().GoToUrl(api);

            }
        }

        public static void youtubeFile(string title, string views, string uploader, string link,string filepath)
        {
            try
            {
                using (System.IO.StreamWriter youtubefile = new System.IO.StreamWriter(@filepath, true))
                {
                    youtubefile.WriteLine(title + "," + views + "," + uploader + "," + link);
                }
            }
            catch(Exception ex)
            {
                throw new ApplicationException("this program did an oopsie: ", ex);
            }
        }
        public static void indeedFile(string jobtitle, string jobLocation, string employer, string filepath)
        {
            try
            {
                using (System.IO.StreamWriter indeedFile = new System.IO.StreamWriter(@filepath, true))
                {
                    indeedFile.WriteLine(jobtitle + "," + jobLocation + "," + employer);
                }
            }
            catch(Exception ex)
            {
                throw new ApplicationException("this program did an oopsie: ", ex);
            }
        }
        public static void imvufile(string id, string api, string filepath)
        {
            try
            {
                using (System.IO.StreamWriter imvufile = new System.IO.StreamWriter(@filepath, true))
                {
                    imvufile.WriteLine(id + "," + api);
                }
            }
            catch(Exception ex)
            {
                throw new ApplicationException("this program did an oopsie: ", ex);
            }
        }

    }
}
