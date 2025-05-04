// src/pages/Public/GuestWelcome.jsx

import './GuestWelcome.css'

const GuestWelcome = () => {
    return (
        <>

            {/* Hero Section */}
            <section className="hero-section">
                <video
                    className="hero-video"
                    autoPlay
                    muted
                    loop
                    playsInline
                >
                    <source src="/media/hero-video.mp4" type="video/mp4" />
                    Your browser does not support the video tag.
                </video>

                <div className="hero-content">
                    <h1>Run with Purpose </h1>
                    <p>Join a vibrant running community. Track progress. Connect with coaches. Race together.</p>
                    <a href="/search" className="hero-button">Explore Events</a>
                </div>
            </section>


        </>
    )
}

export default GuestWelcome


