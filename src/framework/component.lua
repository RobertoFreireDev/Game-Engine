function new_body(i,s,c,t,x,y,x1,y1,w,h)
	local o={i=i,s=s,c=c,t=t,x=x,y=y,box={x=x1,y=y1,w=w,h=h}}
		
	function o:collides(dx,dy,o2)		
		local b=self
			
		if o2.box==nil then
			return
		end
			
		local o1x,o1y,o2x,o2y=
			b.x+b.box.x+dx,
			b.y+b.box.y+dy,
			o2.x+o2.box.x,
			o2.y+o2.box.y
		
		return o1x < o2x + o2.box.w and
			o1x + b.box.w > o2x and
			o1y < o2y + o2.box.h and
			o1y + b.box.h > o2y
	end

	function o:contains(p)
		local b=self
		local bx = b.x + b.box.x
		local by = b.y + b.box.y
		return p.x >= bx and p.x <= bx + b.box.w and
		       p.y >= by and p.y <= by + b.box.h
	end
		
	function o:draw()
		local b=self
		_csprc(b.i,b.s,b.x,b.y,b.c,b.t, 1, 1, false, false)
	end
		
	return o
end

function new_button(i,s,c,t,x,y,x1,y1,w,h)
	local o={}
	o.b = new_body(i,s,c,t,x,y,x1,y1,w,h)

	function o:update() 
		if _mouseclickp(0) and o.b:contains(_mousepos()) then
			o:clicked()
		end
	end
		
	function o:draw()
		o.b:draw()
	end
		
	return o
end

function new_colorbutton(c,x,y,x1,y1,w,h)
	local o = {c=c,x=x,y=y,box={x=x1,y=y1,w=w,h=h}}

	function o:contains(p)
		local bx = o.x + o.box.x
		local by = o.y + o.box.y
		return p.x >= bx and p.x <= bx + o.box.w and
		       p.y >= by and p.y <= by + o.box.h
	end

	function o:update() 
		if _mouseclickp(0) and o:contains(_mousepos()) then
			o:clicked()
		end
	end
		
	function o:draw()
		_rectfill(o.x + o.box.x,o.y + o.box.y,o.box.w,o.box.h,o.c)
	end
		
	return o
end