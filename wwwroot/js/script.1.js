$(document).ready(function(){
	console.log('hello alena')
	$('#home-tab').on("click", function(){
		$('.follower-side').toggle()
		$('.bet-side').toggle()
	})


	$('#profile-tab').on("click", function(){
		$('.follower-side').toggle()
		$('.bet-side').toggle()

		console.log('i clicked the profile tab')
	})

})
