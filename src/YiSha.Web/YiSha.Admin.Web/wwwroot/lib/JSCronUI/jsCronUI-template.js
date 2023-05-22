(function ($) {
	$.fn.jsCronUI.template = function () {
		var value;

		return '<div class="c-schedule-type">' + //c-schedule-type
				'<ul>' +
					'<li>' +
						'<label>' +
							'<input type="radio" value="daily" name="ScheduleType" />' +
							'<span>日</span>' +
						'</label>' +
					'</li>' +
					'<li>' +
						'<label>' +
							'<input type="radio" value="weekly" name="ScheduleType" />' +
							'<span>周</span>' +
						'</label>' +
					'</li>' +
					'<li>' +
						'<label>' +
							'<input type="radio" value="monthly" name="ScheduleType" />' +
							'<span>月</span>' +
						'</label>' +
					'</li>' +
					'<li>' +
						'<label>' +
							'<input type="radio" value="yearly" name="ScheduleType" />' +
							'<span>年</span>' +
						'</label>' +
					'</li>' +
				'</ul>' +
			'</div>' +
			'<div class="c-schedule-options" style="display: none;">' + //c-schedule-options
				'<div class="js-schedule-tod">' +
					'<label>' +
						'<div>时间</div>' +
						'<input type="text" name="time" />' +
					'</label>' +
				'</div>' +
				'<div class="js-schedule-daily">每天的:' + //js-schedule-daily
					'<div>' + 
						'<label>' +
							'<input type="radio" value="daily" name="dailyPattern" />' +
							'<span>自然日</span>' +
						'</label>' +
					'</div>' +
					'<div>' +
						'<label>' +
							'<input type="radio" value="weekday" name="dailyPattern" />' +
							'<span>工作日</span>' +
						'</label>' +
					'</div>' +
				'</div>' +
				'<div class="js-schedule-weekly">每周的:' + //js-schedule-weekly
					'<div name="weeklyDays">' + 
						'<label>' +
							'<input type="checkbox" value="1" />' +
							'<span>星期日</span>' +
						'</label>' +
						'<label>' +
							'<input type="checkbox" value="2" />' +
							'<span>星期一</span>' +
						'</label>' +
						'<label>' +
							'<input type="checkbox" value="3" />' +
							'<span>星期二</span>' +
						'</label>' +
						'<label>' +
							'<input type="checkbox" value="4" />' +
							'<span>星期三</span>' +
						'</label>' +
						'<label>' +
							'<input type="checkbox" value="5" />' +
							'<span>星期四</span>' +
						'</label>' +
						'<label>' +
							'<input type="checkbox" value="6" />' +
							'<span>星期五</span>' +
						'</label>' +
						'<label>' +
							'<input type="checkbox" value="7" />' +
							'<span>星期六</span>' +
						'</label>' +
					'</div>' +
				'</div>' +
				'<div class="js-schedule-monthly">每月的:' + //js-schedule-monthly
					'<div>' + 
						'<label>' +
							'<input type="radio" value="last" name="monthlyPattern" />' +
							'<span>最后一天</span>' +
						'</label>' +
					'</div>' +
					'<div>' +
						'<label>' +
							'<input type="radio" value="week" name="monthlyPattern"/>' +
							'<span></span>' +
							'<select name="weekOccurrence">' +
								'<option value="#1">第一个</option>' +
								'<option value="#2">第二个</option>' +
								'<option value="#3">第三个</option>' +
								'<option value="#4">第四个</option>' +
								'<option value="#5">第五个</option>' +
								'<option value="L">最后一个</option>' +
							'</select>' +
							'<select name="dayOfWeek">' +
								'<option value="1">星期日</option>' +
								'<option value="2">星期一</option>' +
								'<option value="3">星期二</option>' +
								'<option value="4">星期三</option>' +
								'<option value="5">星期四</option>' +
								'<option value="6">星期五</option>' +
								'<option value="7">星期六</option>' +
							'</select>' +
							'<span></span>' +
						'</label>' +
					'</div>' +
					'<div>' +
						'<label>' +
							'<input type="radio" value="date" name="monthlyPattern" />' +
							'<span>指定日</span>' +
							'<input name="date"/>' +
						'</label>' +
					'</div>' +
				'</div>' +
				'<div class="js-schedule-yearly">每年的:' + //js-schedule-yearly
					'<div>' + 
						'<label>' +
							'<input type="radio" name="yearPattern" value="specificDay"/>' +
							'<span>第 </span>' +
							'<select multiple name="monthSpecificDay">' +
								'<option value="1">一月</option>' +
								'<option value="2">二月</option>' +
								'<option value="3">三月</option>' +
								'<option value="4">四月</option>' +
								'<option value="5">五月</option>' +
								'<option value="6">六月</option>' +
								'<option value="7">七月</option>' +
								'<option value="8">八月</option>' +
								'<option value="9">九月</option>' +
								'<option value="10">十月</option>' +
								'<option value="11">十一月</option>' +
								'<option value="12">十二月</option>' +
							'</select>' +
							'<span>，指定日 </span>' +
							'<input name="dayOfMonth"/>' +
						'</label>' +
					'</div>' +
					'<div>' +
						'<label>' +
							'<input type="radio" name="yearPattern" value="weekOccurrence"/>' +
							'<span>第 </span>' +
								'<select multiple name="monthOccurrence">' +
								'<option value="1">一月</option>' +
								'<option value="2">二月</option>' +
								'<option value="3">三月</option>' +
								'<option value="4">四月</option>' +
								'<option value="5">五月</option>' +
								'<option value="6">六月</option>' +
								'<option value="7">七月</option>' +
								'<option value="8">八月</option>' +
								'<option value="9">九月</option>' +
								'<option value="10">十月</option>' +
								'<option value="11">十一月</option>' +
								'<option value="12">十二月</option>' +
							'</select>' +
							'<span>， </span>' +
							'<select name="weekOccurrence">' +
								'<option value="#1">第一个</option>' +
								'<option value="#2">第二个</option>' +
								'<option value="#3">第三个</option>' +
								'<option value="#4">第四个</option>' +
								'<option value="#5">第五个</option>' +
								'<option value="L">最后一个</option>' +
							'</select>' +
							'<select name="dayOfWeek">' +
								'<option value="1">星期日</option>' +
								'<option value="2">星期一</option>' +
								'<option value="3">星期二</option>' +
								'<option value="4">星期三</option>' +
								'<option value="5">星期四</option>' +
								'<option value="6">星期五</option>' +
								'<option value="7">星期六</option>' +
							'</select>' +


						'</label>' +
					'</div>' +
				'</div>' +
			'</div>';
	};
}).call(this, jQuery);
